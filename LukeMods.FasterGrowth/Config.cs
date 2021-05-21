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

    public class Config
    {
        public static readonly Config Instance = new Config();
        
        public float DurationMultiplier { get; set; } = .01F;
        

        private Config()
        {
            var path = FilePath;

            if (File.Exists(path))
            {
                var content = File.ReadAllText(path);
                JsonConvert.PopulateObject(content, this);
            }
            else
            {
                File.WriteAllText(path, JsonConvert.SerializeObject(this));
            }

            Logger.Log(Logger.Level.Debug, "Loaded config, Duration Mul: " + this.DurationMultiplier, null, true);
        }

        static string FilePath => Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            "config.json");

    }

}
