using QModManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.Common
{

    public static class ModUtils
    {

        public static readonly object[] EmptyParams = new object[0];

        public static void LogDebug(string message, bool show = true)
        {
            Logger.Log(Logger.Level.Debug, message, null, show);
        }

    }

}
