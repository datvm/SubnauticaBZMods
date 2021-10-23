using HarmonyLib;
using LukeMods.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UWE;

namespace LukeMods.StorageInterest.Patches
{

    [HarmonyPatch(typeof(StorageContainer))]
    public static class StorageContainerPatches
    {
        static readonly Config config = Config.Instance;
        static readonly Dictionary<StorageContainer, float> InterestTime = new Dictionary<StorageContainer, float>();
        static readonly Dictionary<StorageContainer, float> InterestTimeRequired = new Dictionary<StorageContainer, float>();

        static readonly FieldInfo itemGroups = AccessTools.Field(typeof(ItemsContainer), nameof(itemGroups));

        [HarmonyPatch(nameof(Awake)), HarmonyPostfix]
        public static void Awake(StorageContainer __instance)
        {
            if (!StorageContainerHasInterestEnabled(__instance))
            {
                return;
            }

            var container = __instance.container;
            container.onAddItem += (_) => OnItemChanged(__instance);
            container.onRemoveItem += (_) => OnItemChanged(__instance);

            // Start counting the interest as soon as the game loads.
            ResetTime(__instance);

            // Avoid the need to open a container to start calculating the interest.
            StartInterestRateCalculation(__instance);
        }

        /**
         * Ensure the container we are validating is enabled in the
         * list of containers that will pay interest.
         */
        public static bool StorageContainerHasInterestEnabled(StorageContainer __instance)
        {
            string[] containers = config.GetContainers();
            return containers.Any(storageType => __instance.name.StartsWith(storageType, StringComparison.OrdinalIgnoreCase));
        }

        /**
         * To avoid having to check all Storage types and what would they accept
         * when using the method "HasRoomFor", we are simplifying the logic by
         * using X and Y dimensions against the ammount of items inside the storage.
         */
        public static bool StorageContainerIsFull(StorageContainer __instance)
        {
            // Calculate the available space in this container.
            int containerSize = __instance.container.sizeX * __instance.container.sizeY;
            int itemsAmmount = 0;
            // Calculate the current used spaces inside the container.
            foreach (var item in __instance.container)
            {
                itemsAmmount++;
            }
            // Check if all spaces are used inside the container.
            return (itemsAmmount == containerSize);
        }

        /**
         * Improve the player experience by giving visible feedback. 
         * 
         * This will allow the player to know the current state of
         * each storage interest rate calculation.
         */
        [HarmonyPatch(nameof(OnHandHover)), HarmonyPostfix]
        public static void OnHandHover(GUIHand hand)
        {
            GameObject target = hand.GetActiveTarget();
            StorageContainer __instance = target.GetComponent<StorageContainer>();

            // Assume that all containers start empty.
            string subscript = "Empty";

            // If the container has items:
            if (!__instance.IsEmpty())
            {
                // Get the time information to calculate the interest.
                InterestTime.TryGetValue(__instance, out var timeIn);
                InterestTimeRequired.TryGetValue(__instance, out var timeRequired);
                var timePassed = Time.time - timeIn;

                // Before even starting, to automate the pickup of interest
                // we need to check if the current state means that the
                // pickup is ready and the storage is not full yet.
                // This will only happen when the player looks at the
                // container so we will not generate all items in all
                // containers automatically, causing an inflation of items.
                // This is toggled off by default and the player can change
                // this in the configuration.
                if (!StorageContainerIsFull(__instance) && (timePassed > timeRequired) && config.PayInterestOnCursorHovering)
                {
                    // Pickup the item and start the next calculation.
                    StartInterestRateCalculation(__instance);
                }

                // By default, the interest is ready to be picked up!
                // This is need as we must open the container manually to
                // compute the interest and create the item.
                string interestRate = "Ready!";

                if (!StorageContainerHasInterestEnabled(__instance))
                {
                    interestRate = "Storage disabled!";

                    // When disabling the storage mid-game, we need to
                    // interrupt the current interest in progress.
                    if (timeRequired > 0.0f)
                    {
                        ResetTime(__instance);
                    }
                }
                // When there is no more space for a single 1x1 item, 
                // the storage is full and there is no need to calculate
                // the interest.
                else if (StorageContainerIsFull(__instance))
                {
                    interestRate = "Storage full!";
                }
                // There is an issue with the interest rate not starting
                // properly. By checking if we have no target time, we can
                // identify it and warn the player to take action.
                else if (timeRequired == 0.0f)
                {
                    interestRate = "Open to start!";
                }
                // To improve the player experience, lets display the information
                // about the current interest calculate progress.
                else if (timePassed <= timeRequired)
                {
                    // Simply percentage calculation to help the user.
                    int percentage = (int)((timePassed / timeRequired) * 100);
                    interestRate = percentage.ToString() + "%";
                }

                // Remove the "empty" message in case we are not displaying the
                // disabled information.
                subscript = "";

                // Only report to the player if we have a message or if the
                // interest is disabled but the player choose to see the warning.
                if (StorageContainerHasInterestEnabled(__instance) || config.WarnDisabledInterest)
                {
                    // Prefix the message to give more context to the player.
                    subscript = "Interest rate: " + interestRate ;
                }
            }

            // Add the message to the GUI when pointing the cursor to the storage.
            HandReticle.main.SetText(HandReticle.TextType.HandSubscript, subscript, true, GameInput.Button.None);
        }

        [HarmonyPatch(nameof(Open)), HarmonyPrefix]
        public static void Open(StorageContainer __instance)
        {
            // Whenever the container is open, trigger the interest rate calculation.
            // This way we ensure that when ready the item will be computed and
            // when in progress or not properly started, it will continue or start.
            StartInterestRateCalculation(__instance);
        }

        /**
         * Execute the actions to calculate and pay the interest.
         */
        private static void StartInterestRateCalculation(StorageContainer __instance)
        {
            // Ensure we are only running interest to enabled containers.
            if (!StorageContainerHasInterestEnabled(__instance))
            {
                return;
            }

            if (!InterestTime.TryGetValue(__instance, out var timeIn))
            {
                ResetTime(__instance);
            }

            var container = __instance.container;
            var counter = new Dictionary<TechType, int>();

            // If the container is empty, there is no need to calculate the interest.
            if (__instance.IsEmpty())
            {
                return;
            }

            var maxCount = 0;
            var currTech = TechType.None;
            foreach (var item in container)
            {
                var tech = item.item.GetTechType();

                var count = counter[tech] = counter.GetValueSafe(tech) + 1;
                if (count > maxCount)
                {
                    maxCount = count;
                    currTech = tech;
                }
            }

            // If the container has no space for more items of this type, stop the interest.
            if (!container.HasRoomFor(currTech))
            {
                return;
            }

            var timePassed = Time.time - timeIn;

            var interestRate = config.InterestRate;
            var minTime = config.MinTime;

            // Calculate the required time to compute the next interest payment.
            var timeRequired = Mathf.Max(interestRate / Mathf.Pow(2, maxCount - 1), minTime);
            // Store the required time to use in the GUI.
            InterestTimeRequired[__instance] = timeRequired;

            // Do not create the item while we are still waiting for the required time.
            if (timePassed < timeRequired)
            {
                return;
            }

            // Calls the routine to create execute the payment.
            CoroutineHost.StartCoroutine(PickupItem(__instance, container, currTech));
            // Reset the time so we can iterate in the next interest calculation.
            ResetTime(__instance);
        }

        private static IEnumerator PickupItem(StorageContainer __instance, ItemsContainer container, TechType techType)
        {
            var task = new TaskResult<Pickupable>();
            task.Set(null);

            yield return SpawnAsync(task, techType);

            var pickupable = task.Get();

            if (pickupable != null)
            {
                // Ensure we still have room for this item.
                if (container.HasRoomFor(techType))
                {
                    container.AddItem(pickupable);
                    pickupable.Pickup();
                }

                ResetTime(__instance);
                // If we still have room for more:
                if (container.HasRoomFor(techType))
                {
                    // Start the next interest calculation without the need of
                    // the player closing the container and opening it again,
                    // fixing the issue with some containers not properly
                    // generating new items after it is done once.
                    StartInterestRateCalculation(__instance);
                }
            }
        }

        static IEnumerator SpawnAsync(IOut<Pickupable> result, TechType techType)
        {
            TaskResult<GameObject> prefabResult = new TaskResult<GameObject>();
            yield return CraftData.InstantiateFromPrefabAsync(techType, prefabResult);
            GameObject gameObject = prefabResult.Get();
            if (gameObject == null)
            {
                yield break;
            }
            Pickupable pickupable = gameObject.GetComponent<Pickupable>();
            if (pickupable != null)
            {
                // Removed battery code
            }
            else
            {
                UnityEngine.Object.Destroy(gameObject);
            }
            result.Set(pickupable);
        }

        static void OnItemChanged(StorageContainer __instance)
        {
            ResetTime(__instance);
            // We need to restart the interest after we add or remove an item
            // to the container. When we simply reset it, the interest rate
            // calculation will break and won't restart without calling for it.
            StartInterestRateCalculation(__instance);
        }

        static void ResetTime(StorageContainer __instance)
        {
            // Time used to check if the required time was fulfilled.
            InterestTime[__instance] = Time.time;
            // Initial value to handle the issue with containers not
            // properly started.
            InterestTimeRequired[__instance] = 0.0f;
        }
    }
}
