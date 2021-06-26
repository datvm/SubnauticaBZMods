using HarmonyLib;
using LukeMods.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.FreeBeauty.Patches
{
    [HarmonyPatch(typeof(Base))]
    public static class BasePatches
    {
        static readonly FieldInfo FaceHullStrength = AccessTools.Field(typeof(Base), nameof(FaceHullStrength));
        static readonly FieldInfo CellHullStrength = AccessTools.Field(typeof(Base), nameof(CellHullStrength));

        static bool patched = false;

        static readonly Dictionary<TechType, float> FaceHullStrReplacement = new Dictionary<TechType, float>()
        {
            { TechType.BaseWindow, 0 },
            { TechType.BaseGlassDome, 0 },
            { TechType.BaseLargeGlassDome, 0 },
        };

        static readonly Dictionary<Base.CellType, float> CellHullStrReplacement = new Dictionary<Base.CellType, float>()
        {
            { Base.CellType.Observatory, -1.25f },
        };

        [HarmonyPatch(nameof(Awake)), HarmonyPostfix]
        public static void Awake()
        {
            if (patched)
            {
                return;
            }

            patched = true;

            // Face
            var facesStr = FaceHullStrength.GetValue(null) as float[];
            var faces = Base.FaceToRecipe;
            var len = faces.Length;
            for (int i = 0; i < len; i++)
            {
                if (FaceHullStrReplacement.TryGetValue(faces[i], out var str))
                {
                    facesStr[i] = str;
                }
            }

            // Cell
            var cellsStr = CellHullStrength.GetValue(null) as float[];
            foreach (var cell in CellHullStrReplacement)
            {
                cellsStr[(int)cell.Key] = cell.Value;
            }


        }

        [HarmonyPatch(nameof(GetHullStrength)), HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> GetHullStrength(IEnumerable<CodeInstruction> instructions)
        {
            var isGlass = false;
            
            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldfld)
                {
                    var field = instruction.operand as FieldInfo;
                    if (field.Name == "isGlass")
                    {
                        isGlass = true;
                    }
                }
                else if (isGlass && (instruction.opcode == OpCodes.Brfalse || instruction.opcode == OpCodes.Brfalse_S))
                {
                    isGlass = false;

                    yield return new CodeInstruction(OpCodes.Pop);
                    yield return new CodeInstruction(OpCodes.Ldc_I4_0);
                }

                yield return instruction;
            }
        }

    }
}
