using LukeMods.Common;
using QModManager.API.ModLoading;
using QModManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.FasterPdaScan
{
    [QModCore]
    public static class Initializer
    {

        [QModPatch]
        public static void Init()
        {
            BaseInitializer.Init(nameof(FasterPdaScan), typeof(Initializer).Assembly);
            Logger.Log(Logger.Level.Debug, "Loaded Pda Scan Time Modifier: " + Config.Instance.ScanTimeMultiplier, null, true);
        }

    }
}
