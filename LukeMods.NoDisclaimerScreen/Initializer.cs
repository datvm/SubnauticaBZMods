using HarmonyLib;
using LukeMods.Common;
using QModManager.API.ModLoading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.NoDisclaimerScreen
{
    [QModCore]
    public static class Initializer
    {

        [QModPatch]
        public static void Init()
        {
            BaseInitializer.Init(nameof(NoDisclaimerScreen), typeof(Initializer).Assembly);
        }

    }

    [HarmonyPatch(typeof(EarlyAccessDisclaimer))]
    public class EarlyAccessDisclaimerPatches
    {

        [HarmonyPatch("GetShowTime"), HarmonyPrefix]
        public static bool GetShowTime(ref float __result)
        {
            ModUtils.LogDebug("Patched GetShowTime", false);

            __result = 1E10f; // Has been showing for a few years
            return false;
        }


    }

}
