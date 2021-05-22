using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.Common
{
    public static class BaseInitializer
    {

        public static void Init(string name)
        {
            var harmony = new Harmony(name);
            harmony.PatchAll();
        }

    }
}
