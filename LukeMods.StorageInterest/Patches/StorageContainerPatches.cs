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

            ResetTime(__instance);
        }

        [HarmonyPatch(nameof(Open)), HarmonyPrefix]
        public static void Open(StorageContainer __instance)
        {
            if (!InterestTime.TryGetValue(__instance, out var timeIn))
            {
                return;
            }

            var container = __instance.container;
            var counter = new Dictionary<TechType, int>();

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

            if (maxCount == 0)
            {
                return;
            }

            if (!container.HasRoomFor(currTech))
            {
                return;
            }

            var timePassed = Time.time - timeIn;

            var interestRate = C.InterestRate;
            var minTime = C.MinTime;

            var timeRequired = Mathf.Max(interestRate / Mathf.Pow(2, maxCount - 1), minTime);
            if (timeRequired > timePassed)
            {
                return;
            }

            CoroutineHost.StartCoroutine(PickupItem(container, currTech));
            ResetTime(__instance);
        }

        private static IEnumerator PickupItem(ItemsContainer container, TechType techType)
        {
            var task = new TaskResult<Pickupable>();
            task.Set(null);

            yield return SpawnAsync(task, techType);

            var pickupable = task.Get();

            if (pickupable != null)
            {
                container.AddItem(pickupable);
                pickupable.Pickup();
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

        static void OnItemChanged(StorageContainer container)
        {
            ResetTime(container);
        }

        static void ResetTime(StorageContainer storage)
        {
            InterestTime[storage] = Time.time;
        }

    }

}
