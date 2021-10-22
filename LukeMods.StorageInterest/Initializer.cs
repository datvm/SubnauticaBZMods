using LukeMods.Common;
using QModManager.API.ModLoading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMLHelper.V2.Handlers;

namespace LukeMods.StorageInterest
{
    [QModCore]
    public static class Initializer
    {
        internal static Config config { get; private set; }

        [QModPatch]
        public static void Init()
        {
            BaseInitializer.Init(nameof(StorageInterest), typeof(Initializer).Assembly);

            // Use the SMLHelper config handler to automatic config.json IO and GUI options.
            // This will generate the config.json file if missing and whenever the file is modified
            // it will be correct loaded in the game. When the player changes the values using the
            // options GUI, the values will be written in the file as well.
            config = OptionsPanelHandler.Main.RegisterModOptions<Config>();
        }

    }
}
