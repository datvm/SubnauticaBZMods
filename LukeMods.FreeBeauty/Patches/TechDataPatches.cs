using HarmonyLib;
using LukeMods.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LukeMods.FreeBeauty.Patches
{

    [HarmonyPatch(typeof(TechData))]
    public static class TechDataPatches
    {
        static readonly FieldInfo entries = AccessTools.Field(typeof(TechData), nameof(entries));
        static readonly FieldInfo propertyToID = AccessTools.Field(typeof(TechData), nameof(propertyToID));

        static readonly int propertyIngredients = (int)AccessTools.Field(typeof(TechData), nameof(propertyIngredients)).GetValue(null);
        static readonly int propertyTechType = (int)AccessTools.Field(typeof(TechData), nameof(propertyTechType)).GetValue(null);
        static readonly int propertyAmount = (int)AccessTools.Field(typeof(TechData), nameof(propertyAmount)).GetValue(null);

        static readonly Dictionary<TechType, TechType> IngredientReplacements = new Dictionary<TechType, TechType>()
        {
            { TechType.BaseCorridorGlassI, TechType.BaseCorridorI },
            { TechType.BaseCorridorGlassL, TechType.BaseCorridorL },
            { TechType.BaseObservatory, TechType.BaseRoom },

            { TechType.BaseGlassDome, TechType.BaseCorridorI },
            { TechType.BaseLargeGlassDome, TechType.BaseCorridorI },
            { TechType.BaseWindow, TechType.BaseCorridorI },
        };

        [HarmonyPatch(nameof(Cache)), HarmonyPrefix]
        public static void Cache()
        {
            var e = entries.GetValue(null) as Dictionary<TechType, JsonValue>;

            var ingredientsId = propertyIngredients;
            foreach (var replacement in IngredientReplacements)
            {
                if (!e.TryGetValue(replacement.Key, out var src))
                {
                    continue;
                }

                if (!e.TryGetValue(replacement.Value, out var dest))
                {
                    continue;
                }

                src[ingredientsId] = dest[ingredientsId].Copy();
            }
        }

        static IEnumerable<KeyValuePair<TKey, TValue>> ToEnumerable<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>> enumerator)
        {
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }

    }

}
