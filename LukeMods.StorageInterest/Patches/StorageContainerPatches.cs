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
        static readonly Config C = Config.Instance;
        static readonly Dictionary<StorageContainer, float> InterestTime = new Dictionary<StorageContainer, float>();
        static readonly Dictionary<StorageContainer, float> InterestTimeRequired = new Dictionary<StorageContainer, float>();

        static readonly FieldInfo itemGroups = AccessTools.Field(typeof(ItemsContainer), nameof(itemGroups));

        [HarmonyPatch(nameof(Awake)), HarmonyPostfix]
        public static void Awake(StorageContainer __instance)
        {
            if (!C.Containers.Any(q => __instance.name.IndexOf(q, StringComparison.OrdinalIgnoreCase) > -1))
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
         * Improve the player experience by giving visible feedback. 
         * 
         * This will allow the player to know the current state of
         * each storage interest rate calculation.
         */
        [HarmonyPatch(nameof(OnHandHover)), HarmonyPostfix]
        public static void OnHandHover(GUIHand hand)
        {
            GameObject target = hand.GetActiveTarget();
            StorageContainer container = target.GetComponent<StorageContainer>();

            // Assume all containers start empty.
            string subscript = "Empty";
            // If the container has items:
            if (!container.IsEmpty())
            {
                // Get the time information to calculate the interest.
                InterestTime.TryGetValue(container, out var timeIn);
                InterestTimeRequired.TryGetValue(container, out var timeRequired);
                var timePassed = Time.time - timeIn;

                // By default, the interest is ready to be picked up!
                // This is need as we must open the container manually to
                // compute the interest and create the item.
                string interestRate = "Ready!";

                // There is an issue with the interest rate not starting
                // properly. By checking if we have no target time, we can
                // identify it and warn the player to take action.
                // This may also be used when the container is full.
                if (timeRequired == 0.0f)
                {
                    interestRate = "Open to start!";
                }
                // To improve the player experience, lets display the information
                // about the current interest calculate progress.
                else if (timePassed <= timeRequired)
                {
                    // Simply percentage calculation to help the user.
                    int percentage = (int) ((timePassed / timeRequired) * 100);
                    interestRate = percentage.ToString() + "%";
                }
                // Prefix the message to give more context to the player.
                subscript = "Interest rate: " + interestRate;
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

            var interestRate = C.InterestRate;
            var minTime = C.MinTime;

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
