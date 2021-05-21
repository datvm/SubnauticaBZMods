using HarmonyLib;
using QModManager.API.ModLoading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.FasterGrowth
{

    [QModCore]
    public static class Initializer
    {

        [QModPatch]
        public static void Init()
        {
            var harmony = new Harmony("LVFasterGrowth");
            harmony.PatchAll();
        }

    }

}
