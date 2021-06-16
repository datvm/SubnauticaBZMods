using LukeMods.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.Vitality
{
    public class Config : BaseConfig
    {
        public static readonly Config Instance = new Config();

        public DifficultyConfig Values { get; set; } = GetDefaultModValues();
        public DifficultyConfig ModDefaults { get; set; } = GetDefaultModValues();
        public DifficultyConfig GameDefaults { get; set; } = GetDefaultGameValues();

        private Config()
        {
        }

        static DifficultyConfig GetDefaultGameValues()
        {
            var hardcore = new VitalityConfig()
            {
                Cold = new ColdConfig()
                {
                    Enabled = true,
                    ColdMultiplier = 1f,
                    Max = 100f,
                },
                Health = new HealthConfig()
                {
                    Enabled = true,
                    Max = 100f,
                    DamageMultiplier = 1f,
                    FirstAidKitHeal = 50f,
                },
                Food = new FoodConfig()
                {
                    Enabled = true,
                    FoodIntakeMultiplier = 1,
                    Max = 200f,
                    HungerRate = 1,
                },
                Water = new WaterConfig()
                {
                    Enabled = true,
                    Max = 100f,
                    WaterIntakeMultiplier = 1,
                    DehydrateRate = 1,
                },
                Oxygen = new OxygenConfig()
                {
                    Enabled = true,
                    DeficiencyDepths = new[] { 100f, 200f, },
                    DeficiencyMultipliers = new[] { 1.5f, 3f, },
                    Capacities = new OxygenConfig.OxygenCapacityConfig()
                    {
                        Player = 45,
                        StandardTank = 30,
                        HighCapacityTank = 90,
                        LightweightHighCapacityTank = 90,
                        UltraHighCapacityTank = 180,
                        BoosterTank = 90,
                    },
                },
            };

            var survival = hardcore.Clone();

            var freedom = survival.Clone();
            freedom.Food.Enabled = false;
            freedom.Water.Enabled = false;

            var creative = freedom.Clone();
            creative.Oxygen.Enabled = false;
            creative.Cold.Enabled = false;
            creative.Health.Enabled = false;

            return new DifficultyConfig()
            {
                Hardcore = hardcore,
                Survival = survival,
                Freedom = freedom,
                Creative = creative,
            };
        }

        static DifficultyConfig GetDefaultModValues()
        {
            var result = GetDefaultGameValues();

            var freedom = result.Freedom = result.Survival.Clone();

            freedom.Cold.Max = 200f;
            freedom.Health.FirstAidKitHeal = freedom.Health.Max = 200f;
            freedom.Food.Max = 300f;
            freedom.Water.Max = 200f;
            freedom.Oxygen.Capacities.Player *= 1.5f;
            freedom.Oxygen.Capacities.StandardTank *= 1.5f;
            freedom.Oxygen.Capacities.HighCapacityTank *= 1.5f;
            freedom.Oxygen.Capacities.LightweightHighCapacityTank *= 1.5f;
            freedom.Oxygen.Capacities.UltraHighCapacityTank *= 1.5f;
            freedom.Oxygen.Capacities.BoosterTank *= 1.5f;

            var hardcore = result.Hardcore = result.Survival.Clone();
            hardcore.Cold.Max = 75f;
            hardcore.Cold.ColdMultiplier = 1.25f;
            hardcore.Food.Max = 100f;
            hardcore.Food.FoodIntakeMultiplier = .75f;
            hardcore.Oxygen.DeficiencyDepths = new[] { 50f, 100f };
            freedom.Oxygen.Capacities.StandardTank *= .75f;
            freedom.Oxygen.Capacities.HighCapacityTank *= .75f;
            freedom.Oxygen.Capacities.LightweightHighCapacityTank *= .75f;
            freedom.Oxygen.Capacities.UltraHighCapacityTank *= .75f;
            freedom.Oxygen.Capacities.BoosterTank *= .75f;

            return result;
        }

    }

    public class DifficultyConfig : Clonable
    {
        public VitalityConfig Creative { get; set; }
        public VitalityConfig Freedom { get; set; }
        public VitalityConfig Survival { get; set; }
        public VitalityConfig Hardcore { get; set; }

        public override Clonable Clone()
        {
            var r = base.Clone() as DifficultyConfig;

            r.Creative = this.Creative.Clone();
            r.Freedom = this.Freedom.Clone();
            r.Survival = this.Survival.Clone();
            r.Hardcore = this.Hardcore.Clone();

            return r;
        }

    }

    public class VitalityConfig : Clonable
    {
        public ColdConfig Cold { get; set; }
        public HealthConfig Health { get; set; }
        public FoodConfig Food { get; set; }
        public WaterConfig Water { get; set; }
        public OxygenConfig Oxygen { get; set; }

        public new VitalityConfig Clone()
        {
            var r = (VitalityConfig)base.Clone();

            r.Cold = (ColdConfig)this.Cold.Clone();
            r.Health = (HealthConfig)this.Health.Clone();
            r.Food = (FoodConfig)this.Food.Clone();
            r.Water = (WaterConfig)this.Water.Clone();
            r.Oxygen = (OxygenConfig)this.Oxygen.Clone();

            return r;
        }
    }

    public class ColdConfig : Clonable
    {

        public bool Enabled { get; set; }
        public float Max { get; set; }
        public float ColdMultiplier { get; set; }

    }

    public class HealthConfig : Clonable
    {

        public bool Enabled { get; set; }
        public float Max { get; set; }
        public float DamageMultiplier { get; set; }
        public float FirstAidKitHeal { get; set; }

    }

    public class FoodConfig : Clonable
    {

        public bool Enabled { get; set; }
        public float Max { get; set; }
        public float FoodIntakeMultiplier { get; set; }
        public float HungerRate { get; set; }

    }

    public class WaterConfig : Clonable
    {

        public bool Enabled { get; set; }
        public float Max { get; set; }
        public float WaterIntakeMultiplier { get; set; }
        public float DehydrateRate { get; set; }

    }

    public class OxygenConfig : Clonable
    {

        public bool Enabled { get; set; }

        public float[] DeficiencyDepths { get; set; }
        public float[] DeficiencyMultipliers { get; set; }

        public OxygenCapacityConfig Capacities { get; set; } = new OxygenCapacityConfig();

        public override Clonable Clone()
        {
            var r = (OxygenConfig)base.Clone();
            r.Capacities = (OxygenCapacityConfig)this.Capacities.Clone();

            return r;
        }

        public class OxygenCapacityConfig : Clonable
        {
            public float Player { get; set; }
            public float StandardTank { get; set; }
            public float HighCapacityTank { get; set; }
            public float LightweightHighCapacityTank { get; set; }
            public float UltraHighCapacityTank { get; set; }
            public float BoosterTank { get; set; }

        }

    }

    public abstract class Clonable
    {

        public virtual Clonable Clone()
        {
            return this.MemberwiseClone() as Clonable;
        }

    }

}
