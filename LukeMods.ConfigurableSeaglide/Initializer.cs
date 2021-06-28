using LukeMods.Common;
using QModManager.API.ModLoading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.ConfigurableSeaglide
{
    [QModCore]
    public static class Initializer
    {

        [QModPatch]
        public static void Init()
        {
            BaseInitializer.Init(nameof(ConfigurableSeaglide), typeof(Initializer).Assembly);
            ModUtils.LogDebug("Loaded ConfigurableSeaglide", false);
        }

    }
}
