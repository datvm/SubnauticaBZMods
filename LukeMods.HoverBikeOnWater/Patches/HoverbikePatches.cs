using HarmonyLib;
using LukeMods.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.HoverBikeOnWater.Patches
{

    [HarmonyPatch(typeof(Hoverbike))]
    public static class HoverbikePatches
    {
        static readonly Config C = Config.Instance;

        [HarmonyPatch(nameof(Awake)), HarmonyPrefix]
        public static void Awake(Hoverbike __instance)
        {
            var c = C;

            __instance.topSpeed = c.TopSpeed;
            __instance.enginePowerConsumption = c.EnergyConsumption;
            __instance.toggleLights.energyPerSecond = c.LightEnergyConsumption;

            var rb = __instance.rb;
            rb.drag = c.Drag;
            rb.angularDrag = c.AngularDrag;

            var head = __instance.headManager;
            head.rollSpring = c.RollSpring;
            head.yawSpring = c.YawSpring;
            head.pitchSpring = c.PitchSpring;
            head.rollAngleDeadzone = c.RollAngleDeadzone;

            head.minViewConeAperture = c.MinViewConeAperture;
            head.maxViewConeAperture = c.MaxViewConeAperture;
        }

        [HarmonyPatch(nameof(HoverEngines)), HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> HoverEngines(IEnumerable<CodeInstruction> instructions)
        {
            if (!C.HoverOnWater)
            {
                foreach (var instruction in instructions)
                {
                    yield return instruction;
                }
                yield break;
            }

            var skip = false;

            foreach (var instruction in instructions)
            {
                if (!skip)
                {
                    yield return instruction;
                }

                if (skip)
                {
                    if (instruction.opcode == OpCodes.Stfld && (instruction.operand as FieldInfo)?.Name == "overWater")
                    {
                        skip = false;
                    }
                }
                else
                {
                    if (instruction.opcode == OpCodes.Stfld && (instruction.operand as FieldInfo)?.Name == "overWater")
                    {
                        skip = true;
                    }
                }
            }
        }

    }

    
    

}
