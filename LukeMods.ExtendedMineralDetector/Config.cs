using LukeMods.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.ExtendedMineralDetector
{
    public class Config : BaseConfig
    {
        public static readonly Config Instance = new Config();

        public float PowerConsumption { get; set; } = .5f;
        public float ScanDistance { get; set; } = 200f;

        private Config()
        {
        }

    }
}
