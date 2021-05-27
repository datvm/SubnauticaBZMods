using LukeMods.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.FasterPrawnDrill
{
    public class Config : BaseConfig
    {
        public static readonly Config Instance = new Config();

        public float MaxDrillHealth { get; set; } = 50;
        public float AddOtherDamage { get; set; } = 16;

        private Config()
        {
        }

    }
}
