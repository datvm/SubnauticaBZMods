using LukeMods.Common;
using QModManager.API.ModLoading;
using QModManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.ExtendedMineralDetector
{
    [QModCore]
    public static class Initializer
    {

        [QModPatch]
        public static void Init()
        {
            BaseInitializer.Init(nameof(ExtendedMineralDetector), typeof(Initializer).Assembly);
            Logger.Log(Logger.Level.Debug, "Loaded Scan Distance: " + Config.Instance.ScanDistance, null, true);
        }

    }
}
