using HarmonyLib;
using LukeMods.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.ConfigurableSeaglide.Patches
{

    [HarmonyPatch(typeof(VehicleInterface_MapController))]
    public static class VehicleInterface_MapControllerPatches
    {

        static readonly Config C = Config.Instance;

        [HarmonyPatch(nameof(Update)), HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Update(IEnumerable<CodeInstruction> instructions)
        {
            var shouldPatch = C.mapWorkAboveWater;

            foreach (var instruction in instructions)
            {
                if (shouldPatch)
                {
                    if (instruction.opcode == OpCodes.Br || instruction.opcode == OpCodes.Br_S)
                    {
                        shouldPatch = false;
                    }
                }
                else
                {
                    yield return instruction;
                }
            }
        }

        public static void ShowDebug(bool flag)
        {
            ModUtils.LogDebug("Debug Flag: " + flag.ToString());
        }

    }
}
