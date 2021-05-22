using LukeMods.Common;
using Newtonsoft.Json;
using QModManager.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.FasterGrowth
{

    public class Config : BaseConfig
    {
        public static readonly Config Instance = new Config();
        
        public float DurationMultiplier { get; set; } = .01F;
        
        private Config()
        {
            Logger.Log(Logger.Level.Debug, "Loaded config, Duration Mul: " + this.DurationMultiplier, null, true);
        }

    }

}
