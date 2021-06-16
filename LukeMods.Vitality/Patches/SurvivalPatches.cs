using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LukeMods.Vitality.Patches
{

    [HarmonyPatch(typeof(Survival))]
    public class SurvivalPatches
    {
        static readonly MethodInfo UpdateWarningSounds = AccessTools.Method(typeof(Survival), nameof(UpdateWarningSounds));

        [HarmonyPatch(nameof(Use)), HarmonyPrefix]
        public static bool Use(GameObject useObj, ref bool __result)
        {
            if (useObj != null)
            {
                var techType = CraftData.GetTechType(useObj);

                if (techType == TechType.FirstAidKit)
                {
                    var heal = GameModeUtilsPatches.currentConfig.Health.FirstAidKitHeal;

                    if (Player.main.GetComponent<LiveMixin>().AddHealth(heal) > 0.1f)
                    {
                        __result = true;
                    }
                    else
                    {
                        ErrorMessage.AddMessage(Language.main.Get("HealthFull"));
                        __result = false;
                    }

                    return false;
                }
            }

            return true;
        }

        [HarmonyPatch(nameof(UpdateStats)), HarmonyPrefix]
        public static bool UpdateStats(float timePassed, Survival __instance, ref float __result)
        {
            float num = 0f;
            if (timePassed > float.Epsilon)
            {
                var c = GameModeUtilsPatches.currentConfig;

                float prevVal = __instance.food;
                float prevVal2 = __instance.water;
                float num2 = timePassed / 2520f * 100f * c.Food.HungerRate;
                if (num2 > __instance.food)
                {
                    num += (num2 - __instance.food) * 25f;
                }
                __instance.food = Mathf.Clamp(__instance.food - num2, 0f, c.Food.Max);
                float num3 = timePassed / 1800f * 100f * c.Water.DehydrateRate;
                if (num3 > __instance.water)
                {
                    num += (num3 - __instance.water) * 25f;
                }
                __instance.water = Mathf.Clamp(__instance.water - num3, 0f, c.Water.Max);
                UpdateWarningSounds.Invoke(__instance, new object[] { __instance.foodWarningSounds, __instance.food, prevVal, 20f, 10f });
                UpdateWarningSounds.Invoke(__instance, new object[] { __instance.waterWarningSounds, __instance.water, prevVal2, 20f, 10f });
            }

            __result = num;
            return false;
        }

    }

}
