using LukeMods.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.SeaglideOvercharge
{
    public class Config : BaseConfig
    {
        public static readonly Config Instance = new Config();

        public float OverchargeDuration { get; set; } = 15f;
        public float OverchargeBoost { get; set; } = 2.5f;
        public float OverchargeBatteryCost { get; set; } = 20f;
        public float OverchargeEnergyConsumption { get; set; } = 20f;
        public string OverchargeKey { get; set; } = "t";

        private Config()
        {
        }

    }
}
