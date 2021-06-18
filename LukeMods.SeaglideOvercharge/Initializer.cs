using LukeMods.Common;
using QModManager.API.ModLoading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.SeaglideOvercharge
{
    [QModCore]
    public static class Initializer
    {

        [QModPatch]
        public static void Init()
        {
            BaseInitializer.Init(nameof(SeaglideOvercharge), typeof(Initializer).Assembly);
            ModUtils.LogDebug("Loaded SeaglideOvercharge", false);
        }

    }
}
