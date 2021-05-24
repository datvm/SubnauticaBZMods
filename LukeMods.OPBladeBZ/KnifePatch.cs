using HarmonyLib;
using QModManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.OPBladeBZ
{

    [HarmonyPatch(typeof(Knife), "OnToolUseAnim", new Type[] { typeof(GUIHand), })]
    public class KnifePatch
    {

        public static void Prefix(GUIHand hand, Knife __instance)
        {
            var conf = Config.Instance;
            var data = __instance is HeatBlade ? conf.HeatBlade : conf.Knife;

            __instance.damage = data.Damage;
            __instance.attackDist = data.Range;
            __instance.spikeyTrapDamage = data.SpikyTrapDamage;
        }

    }

}
