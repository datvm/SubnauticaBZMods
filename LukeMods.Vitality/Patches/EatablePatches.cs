using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.Vitality.Patches
{

    [HarmonyPatch(typeof(Eatable))]
    public class EatablePatches
    {

        [HarmonyPatch(nameof(GetFoodValue)), HarmonyPostfix]
        public static void GetFoodValue(ref float __result)
        {
            __result *= GameModeUtilsPatches.currentConfig.Food.FoodIntakeMultiplier;
        }

        [HarmonyPatch(nameof(GetWaterValue)), HarmonyPostfix]
        public static void GetWaterValue(ref float __result)
        {
            __result *= GameModeUtilsPatches.currentConfig.Water.WaterIntakeMultiplier;
        }

    }

}
