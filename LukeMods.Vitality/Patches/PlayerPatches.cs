using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Player;

namespace LukeMods.Vitality.Patches
{

    [HarmonyPatch(typeof(Player))]
    public class PlayerPatches
    {

        static readonly FieldInfo mode = AccessTools.Field(typeof(Player), nameof(mode));

        [HarmonyPatch(nameof(GetOxygenPerBreath)), HarmonyPrefix]
        public static bool GetOxygenPerBreath(float breathingInterval, int depthClass, Player __instance, ref float __result)
        {
            var c = GameModeUtilsPatches.currentConfig.Oxygen;

            if (!c.Enabled)
            {
                __result = 0f;
                return false;
            }

            var num = 1f;
            var mode = (Mode)PlayerPatches.mode.GetValue(__instance);
            if (Inventory.main.equipment.GetCount(TechType.Rebreather) == 0 && mode != Mode.Piloting && mode != Mode.LockedPiloting && __instance.currentWaterPark == null)
            {
                switch (depthClass)
                {
                    case 2:
                        num = c.DeficiencyMultipliers[0];
                        break;
                    case 3:
                        num = c.DeficiencyMultipliers[1];
                        break;
                }
            }

            __result = breathingInterval * num;
            return false;
        }

        [HarmonyPatch(nameof(GetDepthClass)), HarmonyPrefix]
        public static bool GetDepthClass(Player __instance, ref Ocean.DepthClass __result)
        {
            Ocean.DepthClass result = Ocean.DepthClass.Surface;
            CrushDamage crushDamage = null;

            var currSub = __instance.currentSub;
            var mode = (Mode) PlayerPatches.mode.GetValue(__instance);
            var living = (Living)__instance;

            if ((currSub != null && !currSub.isBase) || mode == Mode.LockedPiloting)
            {
                crushDamage = ((!(currSub != null)) ? living.gameObject.GetComponentInParent<CrushDamage>() : currSub.gameObject.GetComponent<CrushDamage>());
            }
            if (crushDamage != null)
            {
                result = crushDamage.GetDepthClass();
                __instance.crushDepth = crushDamage.crushDepth;
            }
            else
            {
                var c = GameModeUtilsPatches.currentConfig.Oxygen;

                __instance.crushDepth = 0f;
                float depthOf = Ocean.GetDepthOf(living.gameObject);
                if (depthOf > c.DeficiencyDepths[1])
                {
                    result = Ocean.DepthClass.Crush;
                }
                else if (depthOf > c.DeficiencyDepths[0])
                {
                    result = Ocean.DepthClass.Unsafe;
                }
                else if (depthOf > __instance.GetSurfaceDepth())
                {
                    result = Ocean.DepthClass.Safe;
                }
            }

            __result = result;
            return false;
        }

    }

}
