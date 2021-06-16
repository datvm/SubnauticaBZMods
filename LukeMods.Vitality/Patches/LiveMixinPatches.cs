using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.Vitality.Patches
{

    [HarmonyPatch(typeof(LiveMixin))]
    public class LiveMixinPatches
    {
        [HarmonyPatch(nameof(TakeDamage)), HarmonyPrefix]
        public static bool TakeDamage(ref float originalDamage, ref bool __result)
        {
            var c = GameModeUtilsPatches.currentConfig.Health;
            if (!c.Enabled)
            {
                __result = false;
                return false;
            }

            originalDamage *= c.DamageMultiplier;
            return true;
        }

        [HarmonyPatch(nameof(maxHealth), MethodType.Getter), HarmonyPrefix]
        public static void maxHealth(LiveMixin __instance)
        {            
            var c = GameModeUtilsPatches.currentConfig.Health;
            __instance.data.maxHealth = c.Max;
        }

    }

}
