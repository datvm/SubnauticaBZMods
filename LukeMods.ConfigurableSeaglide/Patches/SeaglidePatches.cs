using HarmonyLib;
using LukeMods.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.ConfigurableSeaglide.Patches
{

    [HarmonyPatch(typeof(Seaglide))]
    public static class SeaglidePatches
    {

        static readonly Config C = Config.Instance;

        [HarmonyPatch(nameof(Start)), HarmonyPostfix]
        public static void Start(Seaglide __instance)
        {
            var c = C;

            __instance.toggleLights.energyPerSecond = c.lightEnergy;
        }

        [HarmonyPatch(nameof(UpdateEnergy)), HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> UpdateEnergy(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldc_R4 && (float)instruction.operand == .1f)
                {
                    var r = new CodeInstruction(instruction);
                    r.operand = C.energy;

                    yield return r;
                }
                else
                {
                    yield return instruction;
                }
            }
        }

    }

}
