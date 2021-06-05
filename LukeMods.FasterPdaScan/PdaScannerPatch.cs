using HarmonyLib;
using QModManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LukeMods.FasterPdaScan
{

    [HarmonyPatch(typeof(PDAScanner), "Scan")]
    public class PdaScannerPatch
    {
        static bool updated = false;
        static readonly FieldInfo MappingField = typeof(PDAScanner).GetField("mapping", BindingFlags.NonPublic | BindingFlags.Static);
        static readonly FieldInfo CompleteField = typeof(PDAScanner).GetField("complete", BindingFlags.NonPublic | BindingFlags.Static);

        [HarmonyPrefix]
        public static void Prefix()
        {
            var multiplier = Config.Instance.ScanTimeMultiplier;
            
            // For scanned fragments
            var techType = PDAScanner.scanTarget.techType;
            var scanned = (CompleteField.GetValue(null) as HashSet<TechType>)?.Contains(techType) == true;

            if (scanned && PDAScanner.scanTarget.progress == 0)
            {
                Logger.Log(Logger.Level.Debug, "Reducing fragment time", null, true);

                var gameObject = PDAScanner.scanTarget.gameObject;
                if (gameObject != null)
                {
                    gameObject.SendMessage("OnScanBegin", UnityEngine.SendMessageOptions.DontRequireReceiver);
                }

                PDAScanner.scanTarget.progress = 1 - multiplier;
                return;
            }

            // For non-scanned: try to update all
            if (updated)
            {
                return;
            }

            var mapping = MappingField.GetValue(null) as Dictionary<TechType, PDAScanner.EntryData>;

            if (mapping == null)
            {
                return;
            }

            Logger.Log(Logger.Level.Debug, "Reducing scan time", null, true);

            foreach (var item in mapping)
            {
                item.Value.scanTime *= multiplier;
            }

            updated = true;
        }

    }

}
