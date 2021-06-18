using LukeMods.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.PropulseEverything
{
    public class Config : BaseConfig
    {
        public static readonly Config Instance = new Config();

        public float EnergyRate { get; set; } = .5f;
        public float PickupDistance { get; set; } = 50f;
        public float AttractionForce { get; set; } = 350f;
        public float ShootForce { get; set; } = 150f;
        public float MassScalingFactor { get; set; } = 0.001f;

        private Config()
        {
        }

    }
}
