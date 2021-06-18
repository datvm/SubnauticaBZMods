using HarmonyLib;
using LukeMods.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.SeaglideOvercharge.Patches
{

    [HarmonyPatch(typeof(PlayerController))]
    public static class PlayerControllerPatches
    {
        [HarmonyPatch(nameof(SetMotorMode)), HarmonyPostfix]
        public static void SetMotorMode(Player.MotorMode newMotorMode, PlayerController __instance)
        {
            if (newMotorMode == Player.MotorMode.Seaglide)
            {
                var info = SeaglidePatches.recentOvercharge;
                if (info == null || info.overchargeTime <= 0)
                {
                    return;
                }

                var mul = info.overchargeMul;
                var motor = __instance.underWaterController;
                motor.forwardMaxSpeed *= mul;
                motor.backwardMaxSpeed *= mul;
                motor.strafeMaxSpeed *= mul;
                motor.verticalMaxSpeed *= mul;
                motor.waterAcceleration *= mul;
                motor.swimDrag /= mul;
            }
        }

    }

}
