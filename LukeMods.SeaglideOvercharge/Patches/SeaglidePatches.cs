using HarmonyLib;
using LukeMods.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LukeMods.SeaglideOvercharge.Patches
{

    [HarmonyPatch(typeof(Seaglide))]
    public static class SeaglidePatches
    {
        static readonly FieldInfo activeState = AccessTools.Field(typeof(Seaglide), nameof(activeState));
        static readonly FieldInfo energyMixin = AccessTools.Field(typeof(Seaglide), nameof(energyMixin));

        static readonly MethodInfo SetMotorMode = AccessTools.Method(typeof(Player), nameof(SetMotorMode));

        static bool AnnouncementShown = false;
        static Dictionary<Seaglide, SeaglideOverchargeInfo> info = new Dictionary<Seaglide, SeaglideOverchargeInfo>();

        public static SeaglideOverchargeInfo recentOvercharge;

        static Config C = Config.Instance;

        public static SeaglideOverchargeInfo GetOrCreateInfo(this Seaglide seaglide)
        {
            if (!info.TryGetValue(seaglide, out var result))
            {
                result = info[seaglide] = new SeaglideOverchargeInfo(seaglide);
            }

            return result;
        }

        [HarmonyPatch(nameof(Update)), HarmonyPostfix]
        public static void Update(Seaglide __instance)
        {
            var info = __instance.GetOrCreateInfo();
            if (info.overchargeTime > 0)
            {
                info.overchargeTime = Mathf.Max(0, info.overchargeTime - Time.deltaTime);

                if (info.overchargeTime <= 0)
                {
                    ResetMotorMode();
                }
            }

            if ((bool)activeState.GetValue(__instance))
            {
                if (!AnnouncementShown)
                {
                    AnnouncementShown = true;
                    ErrorMessage.AddError($"Press {C.OverchargeKey.ToUpper()} to activate Overcharge.");
                }

                if (Input.GetKeyDown(C.OverchargeKey.ToLower()))
                {
                    TryActivatingOvercharge(info);
                }
            }
        }

        static void TryActivatingOvercharge(SeaglideOverchargeInfo info)
        {
            // Only activate if none is activated
            if (info.overchargeTime > 0)
            {
                ErrorMessage.AddError("Seaglide is still in Overcharged mode");
                return;
            }

            // Check battery
            var c = C;
            var batteryConsumption = c.OverchargeBatteryCost;
            var eMixin = (EnergyMixin)energyMixin.GetValue(info.seaglide);
            var battery = eMixin.GetBattery() as Battery;
            if (battery == null || battery.charge <= batteryConsumption)
            {
                ErrorMessage.AddError("Not enough battery charge");
                return;
            }

            var player = Player.main;
            if (player == null)
            {
                ModUtils.LogDebug("Player null");
                return;
            }

            // Boost
            info.overchargeTime = c.OverchargeDuration;
            info.overchargeMul = c.OverchargeBoost;
            eMixin.ConsumeEnergy(batteryConsumption);
            battery._capacity -= batteryConsumption;

            recentOvercharge = info;

            // Set new Motor Mode to player to update speed
            ResetMotorMode(player);

            ErrorMessage.AddError($"Seaglide Overcharged for {(int)info.overchargeTime} seconds.");
        }

        static void ResetMotorMode(Player player = null)
        {
            if (player == null) { player = Player.main; }
            if (player == null) { return; }

            SetMotorMode.Invoke(player, new object[] { Player.MotorMode.Dive, });
            SetMotorMode.Invoke(player, new object[] { Player.MotorMode.Seaglide, });
        }

    }

    public class SeaglideOverchargeInfo
    {

        public Seaglide seaglide;

        public float overchargeTime = 0f;
        public float overchargeMul = 0f;

        public SeaglideOverchargeInfo(Seaglide seaglide)
        {
            this.seaglide = seaglide;
        }

        public void Update()
        {

        }

    }

}
