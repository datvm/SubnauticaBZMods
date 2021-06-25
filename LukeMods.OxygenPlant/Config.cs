using LukeMods.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.OxygenPlant
{
    public class Config : BaseConfig
    {
        public static readonly Config Instance = new Config();

        public float Duration { get; set; } = 60f;
        public float Capacity { get; set; } = 70f;

        private Config()
        {
        }

    }
}
