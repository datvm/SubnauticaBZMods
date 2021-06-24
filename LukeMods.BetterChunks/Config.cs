using LukeMods.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.BetterChunks
{
    public class Config : BaseConfig
    {
        public static readonly Config Instance = new Config();

        public float[] SpawnChances { get; set; } = { 1, 1, .5f, .1f };
        public float[] Silver { get; set; } = { 1, .5f, .1f, 0 };
        public float[] Gold { get; set; } = { 1, .5f, .1f, 0 };
        public float[] Lead { get; set; } = { 1, .5f, .1f, 0 };
        public float[] Copper { get; set; } = { 1, .5f, .1f, 0 };

        private Config()
        {
        }

    }
}
