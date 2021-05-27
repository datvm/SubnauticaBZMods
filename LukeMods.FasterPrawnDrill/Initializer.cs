using LukeMods.Common;
using QModManager.API.ModLoading;
using QModManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.FasterPrawnDrill
{
    [QModCore]
    public static class Initializer
    {

        [QModPatch]
        public static void Init()
        {
            BaseInitializer.Init(nameof(FasterPrawnDrill), typeof(Initializer).Assembly);
            Logger.Log(Logger.Level.Debug, "Loaded Drill damage: " + Config.Instance.MaxDrillHealth, null, true);
        }

    }
}
