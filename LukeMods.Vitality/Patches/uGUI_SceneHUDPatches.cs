using HarmonyLib;
using LukeMods.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.Vitality.Patches
{

    
    [HarmonyPatch(typeof(uGUI_SceneHUD))]
    public class uGUI_SceneHUDPatches
    {
        static readonly object[] EmptyParams = ModUtils.EmptyParams;


        static readonly MethodInfo ResetHUD = AccessTools.Method(typeof(uGUI_SceneHUD), nameof(ResetHUD));
        static readonly MethodInfo UpdateElements = AccessTools.Method(typeof(uGUI_SceneHUD), nameof(UpdateElements));
        static readonly FieldInfo _mode = AccessTools.Field(typeof(uGUI_SceneHUD), nameof(_mode));
        static readonly FieldInfo[] _shows = new string[] { "_showOxygen", "_showHealth", "_showFood", "_showWater", "_showCold", }
            .Select(q => AccessTools.Field(typeof(uGUI_SceneHUD), q))
            .ToArray();

        [HarmonyPatch(nameof(GameModeChanged)), HarmonyPrefix]
        public static bool GameModeChanged(GameModeOption gameMode, uGUI_SceneHUD __instance)
        {
            ResetHUD.Invoke(__instance, EmptyParams);

            _mode.SetValue(__instance, 2);
            foreach (var _show in _shows)
            {
                _show.SetValue(__instance, true);
            }

            UpdateElements.Invoke(__instance, EmptyParams);

            return false;
        }

    }

}
