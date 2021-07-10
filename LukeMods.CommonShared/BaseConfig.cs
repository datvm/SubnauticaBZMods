using Newtonsoft.Json;
using QModManager.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.Common
{

    public abstract class BaseConfig
    {

        protected BaseConfig()
        {
            var path = FilePath;

            if (File.Exists(path))
            {
                var content = File.ReadAllText(path);
                JsonConvert.PopulateObject(content, this);
            }

            // Always write the config file back to update it
            File.WriteAllText(path, JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        string FilePath => Path.Combine(
            Path.GetDirectoryName(this.GetType().Assembly.Location),
            "config.json");

    }

}
