using LukeMods.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;

namespace LukeMods.StorageInterest
{
    // This will generate a section in the Options window at the Mods tab.
    [Menu("Storage with Interest")]
    public class Config : ConfigFile
    {
        public static Config Instance;

        // Create a slider in the options section of this mod.
        [Slider("Interest Rate", 10.0f, 600.0f, DefaultValue = 300.0f)]
        public float InterestRate = 300.0f;

        // Create a slider in the options section of this mod.
        [Slider("Shortes Rate", 10.0f, 600.0f, DefaultValue = 10.0f)]
        public float MinTime = 10.0f;

        public string[] Containers { get; set; } = new[] { "SmallLocker", "Aquarium", "Locker", "StorageContainer" };

        public Config()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
    }
}
