using LukeMods.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.FasterPdaScan
{
    public class Config : BaseConfig
    {
        public static readonly Config Instance = new Config();

        public float ScanTimeMultiplier { get; set; } = .5f;

        private Config()
        {
        }

    }
}
