using HarmonyLib;
using LukeMods.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.Vitality.Patches
{

    [HarmonyPatch(typeof(Oxygen))]
    public class OxygenPatches
    {

        [HarmonyPatch(nameof(Start)), HarmonyPrefix]
        public static void Start(Oxygen __instance)
        {
            SetCapacity(__instance);
        }

        [HarmonyPatch(nameof(GetOxygenCapacity)), HarmonyPrefix]
        public static void GetOxygenCapacity(Oxygen __instance)
        {
            SetCapacity(__instance);
        }

        static void SetCapacity(Oxygen __instance)
        {
            var c = GameModeUtilsPatches.currentConfig.Oxygen.Capacities;

            if (__instance.isPlayer)
            {
                __instance.oxygenCapacity = c.Player;
            }
            else
            {
                var item = __instance.gameObject.GetComponent<Pickupable>();
                var techType = item.GetTechType();

                var value = -1f;

                switch (techType)
                {
                    case TechType.Tank:
                        value = c.StandardTank;
                        break;
                    case TechType.DoubleTank:
                        value = c.HighCapacityTank;
                        break;
                    case TechType.PlasteelTank:
                        value = c.LightweightHighCapacityTank;
                        break;
                    case TechType.HighCapacityTank:
                        value = c.UltraHighCapacityTank;
                        break;
                    case TechType.SuitBoosterTank:
                        value = c.BoosterTank;
                        break;
                }
                
                if (value != -1)
                {
                    __instance.oxygenCapacity = value;
                }
            }
        }

    }
}
