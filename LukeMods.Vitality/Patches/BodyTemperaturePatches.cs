using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.Vitality.Patches
{

    [HarmonyPatch(typeof(BodyTemperature))]
    public class BodyTemperaturePatches
    {
        static readonly FieldInfo coldMeterMaxValue = AccessTools.Field(typeof(BodyTemperature), nameof(coldMeterMaxValue));

        [HarmonyPatch(nameof(AddCold)), HarmonyPrefix]
        public static bool AddCold(ref float cold)
        {
            var c = GameModeUtilsPatches.currentConfig.Cold;

            if (!c.Enabled)
            {
                return false;
            }

            cold *= c.ColdMultiplier;
            return true;
        }

        [HarmonyPatch(nameof(Update)), HarmonyPrefix]
        public static void Update(BodyTemperature __instance)
        {
            var c = GameModeUtilsPatches.currentConfig.Cold;

            coldMeterMaxValue.SetValue(__instance, c.Max);
        }

    }

}
