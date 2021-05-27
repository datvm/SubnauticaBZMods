using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LukeMods.FasterPrawnDrill
{

    [HarmonyPatch(typeof(ExosuitDrillArm), "OnHit")]
    public class ExosuitDrillArmPatch
    {
        static readonly FieldInfo ExosuitField = typeof(ExosuitDrillArm).GetField("exosuit", BindingFlags.NonPublic | BindingFlags.Instance);
        static readonly FieldInfo DrillingField = typeof(ExosuitDrillArm).GetField("drilling", BindingFlags.NonPublic | BindingFlags.Instance);

        [HarmonyPrefix]
        public static void Prefix(ExosuitDrillArm __instance)
        {
            try
            {
                var damage = Config.Instance.AddOtherDamage;
                if (damage <= 0)
                {
                    return;
                }

                var exosuit = ExosuitField.GetValue(__instance) as Exosuit;

                if (exosuit == null)
                {
                    QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Error, "Error: exosuit is null", null, true);
                    return;
                }

                if (exosuit.CanPilot() && exosuit.GetPilotingMode())
                {
                    Vector3 zero = Vector3.zero;
                    GameObject gameObject = null;
                    Vector3 vector;
                    UWE.Utils.TraceFPSTargetPosition(exosuit.gameObject, 5f, ref gameObject, ref zero, out vector, true);

                    if (gameObject != null && gameObject && (bool)DrillingField.GetValue(__instance))
                    {
                        Drillable drillable = gameObject.FindAncestor<Drillable>();

                        if (!drillable)
                        {
                            LiveMixin liveMixin = gameObject.FindAncestor<LiveMixin>();
                            if (liveMixin)
                            {
                                liveMixin.IsAlive();
                                liveMixin.TakeDamage(damage, zero, DamageType.Drill, null);

                                QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, "Drill Extra Damaging: " + damage, null, true);

                                // Leave this for the real one
                                // this.drillTarget = gameObject;
                            }
                            return;
                        }

                        var maxHealth = Config.Instance.MaxDrillHealth;
                        var healths = drillable.health;

                        QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, $"Hitting drillable with healths: " + healths.Length, null, true);

                        for (int i = 0; i < healths.Length; i++)
                        {
                            if (healths[i] > maxHealth)
                            {
                                QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Debug, $"Drillable health reduced from {healths[i]} to {maxHealth}", null, true);

                                healths[i] = maxHealth;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                QModManager.Utility.Logger.Log(QModManager.Utility.Logger.Level.Error, "Error: " + ex.ToString(), ex, true);
            }
            
        }

    }
}
