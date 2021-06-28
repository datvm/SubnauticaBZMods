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
            //var shouldPatch = C.mapWorkAboveWater;

            return new CodeMatcher(instructions)
                .MatchForward(false,
                    new CodeMatch(OpCodes.Ldc_R4, .4f)
                )
                .SetOperandAndAdvance(1E10f)
                .InstructionEnumeration();
        }

        public static void ShowDebug(bool flag)
        {
            ModUtils.LogDebug("Debug Flag: " + flag.ToString());
        }

    }
}
