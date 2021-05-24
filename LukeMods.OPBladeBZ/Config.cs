using LukeMods.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.OPBladeBZ
{

    public class Config : BaseConfig
    {
        public static readonly Config Instance = new Config();

        public KnifeConfig Knife { get; set; } = new KnifeConfig() { Range = 10, Damage = 80, SpikyTrapDamage = 4f, };
        public KnifeConfig HeatBlade { get; set; } = new KnifeConfig() { Range = 10, Damage = 40, SpikyTrapDamage = 8f, };

        private Config()
        {
        }

        public class KnifeConfig
        {
            public float Range { get; set; } = 10F;
            public float Damage { get; set; } = 80F;
            public float SpikyTrapDamage { get; set; } = 4f;
        }

    }

}
