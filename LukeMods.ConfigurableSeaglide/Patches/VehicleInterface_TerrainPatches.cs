using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.ConfigurableSeaglide.Patches
{
    
    [HarmonyPatch(typeof(VehicleInterface_Terrain))]
    public static class VehicleInterface_TerrainPatches
    {

        static readonly Config C = Config.Instance;

        [HarmonyPatch(nameof(Start)), HarmonyPrefix]
        public static void Start(VehicleInterface_Terrain __instance)
        {
            var c = C;

            __instance.hologramRadius = c.hologramRadius;
            __instance.mapWorldRadius = c.mapWorldRadius;
        }

    }
}
