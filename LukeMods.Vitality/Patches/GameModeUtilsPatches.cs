using HarmonyLib;
using LukeMods.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.Vitality.Patches
{

    [HarmonyPatch(typeof(GameModeUtils))]
    public class GameModeUtilsPatches
    {

        public static DifficultyConfig C = Config.Instance.Values;
        public static VitalityConfig currentConfig = C.Freedom;

        [HarmonyPatch(nameof(SetGameMode)), HarmonyPrefix]
        public static void SetGameMode(GameModeOption mode, GameModeOption cheats)
        {
            currentConfig = TranslateConfig(mode);

            var name = "Creative";
            if (currentConfig == C.Freedom)
            {
                name = "Freedom";
            }
            else if (currentConfig == C.Hardcore)
            {
                name = "Hardcore";
            }
            else if (currentConfig == C.Survival)
            {
                name = "Survival";
            }

            ModUtils.LogDebug("Switching game mode to " + name);
        }

        [HarmonyPatch(nameof(AllowsAchievements)), HarmonyPrefix]
        public static bool AllowsAchievements(ref bool __result)
        {
            __result = true;
            return false;
        }

        [HarmonyPatch(nameof(IsOptionActive), new Type[] { typeof(GameModeOption), typeof(GameModeOption) }), HarmonyPrefix]
        public static bool IsOptionActive(GameModeOption mode, GameModeOption option, ref bool __result)
        {
            var currentConfig = TranslateConfig(mode);
            var shouldRunOriginal = false;

            switch (option)
            {
                case GameModeOption.NoSurvival:
                    __result = !currentConfig.Food.Enabled && !currentConfig.Water.Enabled;
                    break;
                case GameModeOption.NoOxygen:
                    __result = !currentConfig.Oxygen.Enabled;
                    break;
                case GameModeOption.NoCold:
                    __result = !currentConfig.Cold.Enabled;
                    break;
                case GameModeOption.NoAggression:
                    __result = !currentConfig.Health.Enabled;
                    break;
                default:
                    shouldRunOriginal = true;
                    break;
            }

            return shouldRunOriginal;
        }

        static VitalityConfig TranslateConfig(GameModeOption gameMode)
        {
            switch (gameMode)
            {
                case GameModeOption.Survival:
                    return C.Survival;
                case GameModeOption.Hardcore:
                    return C.Hardcore;
                case GameModeOption.Freedom:
                    return C.Freedom;
                case GameModeOption.Creative:
                default:
                    return C.Creative;
            }
        }

    }

}
