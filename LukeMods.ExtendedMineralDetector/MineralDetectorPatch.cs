using HarmonyLib;
using QModManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.ExtendedMineralDetector
{

    [HarmonyPatch(typeof(MetalDetector), "Start")]
    public class MineralDetectorPatch
    {

        [HarmonyPrefix]
        public static void Prefix(MetalDetector __instance)
        {
            var conf = Config.Instance;

            __instance.powerConsumption = conf.PowerConsumption;
            __instance.scanDistance = conf.ScanDistance;
        }

    }
}
