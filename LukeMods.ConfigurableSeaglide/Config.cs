using LukeMods.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.ConfigurableSeaglide
{
    public class Config : BaseConfig
    {
        public static readonly Config Instance = new Config();

        public float energy { get; set; } = .1f;
        public float lightEnergy { get; set; } = .06f;

        public float hologramRadius { get; set; } = 1f;
        public int mapWorldRadius { get; set; } = 20;
        public bool mapWorkAboveWater { get; set; } = false;

        public float seaglideForwardMaxSpeed { get; set; } = 25f;
        public float seaglideBackwardMaxSpeed { get; set; } = 6.35f;
        public float seaglideStrafeMaxSpeed { get; set; } = 6.35f;
        public float seaglideVerticalMaxSpeed { get; set; } = 6.34f;
        public float seaglideWaterAcceleration { get; set; } = 36.56f;
        public float seaglideSwimDrag { get; set; } = 2f;

        private Config()
        {
        }

    }
}
