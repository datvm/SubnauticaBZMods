using LukeMods.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.HoverBikeOnWater
{
    public class Config : BaseConfig
    {
        public static readonly Config Instance = new Config();

        public bool HoverOnWater { get; set; } = true;

        public float TopSpeed { get; set; } = 11f;
        public float Drag { get; set; } = .8f;
        public float AngularDrag { get; set; } = 1f;
        public float PitchSpring { get; set; } = 5f;

        public float YawSpring { get; set; } = 360f;
        public float MinViewConeAperture { get; set; } = 0f;
        public float MaxViewConeAperture { get; set; } = 10f;

        public float RollSpring { get; set; } = 2.5f;
        public float RollAngleDeadzone { get; set; } = 45f;

        public float EnergyConsumption { get; set; } = 0.06666f;
        public float LightEnergyConsumption { get; set; } = 0f;

        public string SummonKey { get; set; } = "f";
        public float SummonEnergyPerMeter { get; set; } = .1f;

        public float MaxHealth { get; set; } = 200f;

        private Config()
        {
        }

    }


}
