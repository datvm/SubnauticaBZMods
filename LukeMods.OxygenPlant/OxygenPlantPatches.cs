using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OP = global::OxygenPlant;

namespace LukeMods.OxygenPlant
{
    [HarmonyPatch(typeof(OP))]
    public static class OxygenPlantPatches
    {

        static readonly Config C = Config.Instance;

        [HarmonyPatch(nameof(GetProgress)), HarmonyPrefix]
        public static void GetProgress(OP __instance)
        {
            SetValues(__instance);
        }

        [HarmonyPatch(nameof(OnHandClick)), HarmonyPrefix]
        public static void OnHandClick(OP __instance)
        {
            SetValues(__instance);
        }

        static void SetValues(OP __instance)
        {
            var c = C;
            __instance.duration = c.Duration;
            __instance.capacity = c.Capacity;
        }

    }

}
