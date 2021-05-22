using LukeMods.Common;
using QModManager.API.ModLoading;
using QModManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.OPBladeBZ
{
    [QModCore]
    public static class Initializer
    {

        [QModPatch]
        public static void Init()
        {
            BaseInitializer.Init("LVFasterGrowth", typeof(Initializer).Assembly);
            Logger.Log(Logger.Level.Debug, "Loaded Blade damage: " + Config.Instance.Knife.Damage, null, true);
        }

    }
}
