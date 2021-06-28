using HarmonyLib;
using LukeMods.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.ConfigurableSeaglide.Patches
{

    [HarmonyPatch(typeof(PlayerController))]
    public static class PlayerControllerPatches
    {

        static readonly Config C = Config.Instance;

        [HarmonyPatch(nameof(SetMotorMode)), HarmonyPrefix]
        public static void SetMotorMode(Player.MotorMode newMotorMode, PlayerController __instance)
        {
            if (newMotorMode != Player.MotorMode.Seaglide)
            {
                return;
            }

            var c = C;
            __instance.seaglideForwardMaxSpeed = c.seaglideForwardMaxSpeed;
            __instance.seaglideBackwardMaxSpeed = c.seaglideBackwardMaxSpeed;
            __instance.seaglideStrafeMaxSpeed = c.seaglideStrafeMaxSpeed;
            __instance.seaglideVerticalMaxSpeed = c.seaglideVerticalMaxSpeed;
            __instance.seaglideWaterAcceleration = c.seaglideWaterAcceleration;
            __instance.seaglideSwimDrag = c.seaglideSwimDrag;
        }

    }

}
