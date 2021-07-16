using LukeMods.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.StorageInterest
{
    public class Config : BaseConfig
    {
        public static readonly Config Instance = new Config();

        public float InterestRate { get; set; } = 300f;
        public float MinTime { get; set; } = 10f;
        public string[] Containers { get; set; } = new[] { "SmallLocker", "Aquarium", "Locker", "StorageContainer" };

        private Config()
        {
        }

    }

}
