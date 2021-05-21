using HarmonyLib;
using QModManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.FasterGrowth
{

    [HarmonyPatch(typeof(GrowingPlant), "GetGrowthDuration")]
    public class GrowingPlantPatcher
    {
        public static void Postfix(ref float __result)
        {
            __result *= Config.Instance.DurationMultiplier;
        }

    }

}
