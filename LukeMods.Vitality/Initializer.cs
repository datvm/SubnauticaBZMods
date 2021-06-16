using LukeMods.Common;
using QModManager.API.ModLoading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.Vitality
{
    [QModCore]
    public static class Initializer
    {

        [QModPatch]
        public static void Init()
        {
            Config.Instance.ToString(); // Just to get the file
            BaseInitializer.Init(nameof(Vitality), typeof(Initializer).Assembly);
            ModUtils.LogDebug("Loaded Vitality", false);
        }

    }
}
