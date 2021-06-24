using HarmonyLib;
using LukeMods.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace LukeMods.BetterChunks.Patches
{

    [HarmonyPatch(typeof(BreakableResource))]
    public static class BreakableResourcePatches
    {
        static Config C = Config.Instance;
        static readonly Random random = new Random();

        static int currIndex = 0;

        // Don't change the nameof
        [HarmonyPatch(nameof(BreakIntoResources)), HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> BreakIntoResourcesTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            var alreadyChanged = false;

            foreach (var instruction in instructions)
            {
                ModUtils.LogDebug(instruction.opcode.Name, false);

                if (!alreadyChanged && instruction.opcode == OpCodes.Ldc_I4_0)
                {
                    alreadyChanged = true;
                    yield return new CodeInstruction(OpCodes.Ldc_I4_1);
                }
                else
                {
                    yield return instruction;
                }

            }
        }

        [HarmonyPatch(nameof(BreakIntoResources)), HarmonyPrefix]
        public static void BreakIntoResources(BreakableResource __instance)
        {
            __instance.numChances = GetSpawnNumber();

            currIndex = 0;
        }        

        [HarmonyPatch(nameof(ChooseRandomResource)), HarmonyPostfix]
        public static void ChooseRandomResource(BreakableResource __instance, ref AssetReferenceGameObject __result)
        {
            var prefabs = __instance.prefabList;

            if (prefabs.Count != 1)
            {
                return;
            }

            var prefab = prefabs[0];
            var c = C;
            float[] chances;


            switch (prefab.prefabTechType)
            {
                case TechType.Silver:
                    chances = c.Silver;
                    break;
                case TechType.Gold:
                    chances = c.Gold;
                    break;
                case TechType.Copper:
                    chances = c.Copper;
                    break;
                case TechType.Lead:
                    chances = c.Lead;
                    break;
                default:
                    return;
            }

            if (currIndex >= chances.Length)
            {
                return;
            }

            var roll = random.NextDouble();
            __result = roll > chances[currIndex++] ? __instance.defaultPrefabReference : prefab.prefabReference;
        }

        public static int GetSpawnNumber()
        {
            var result = 0;

            var chances = C.SpawnChances;
            var len = chances.Length;
            for (int i = 0; i < len; i++)
            {
                if (random.NextDouble() > chances[i]) { break; }
                else { result++; }
            }

            return result;
        }

    }
}
