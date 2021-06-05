using LukeMods.Common;
using QModManager.API.ModLoading;
using QModManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.MineralDetectorBlip
{
    [QModCore]
    public static class Initializer
    {

        [QModPatch]
        public static void Init()
        {
            BaseInitializer.Init(nameof(MineralDetectorBlip), typeof(Initializer).Assembly);
            Logger.Log(Logger.Level.Debug, "Loaded Mineral Detector Blip", null, false);
        }

    }
}
