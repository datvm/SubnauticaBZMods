using HarmonyLib;
using LukeMods.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LukeMods.HoverBikeOnWater.Patches
{

    [HarmonyPatch(typeof(Hoverpad))]
    public static class HoverPadPatches
    {
        static readonly Config C = Config.Instance;

        static readonly MethodInfo TryDockBike = AccessTools.Method(typeof(Hoverpad), nameof(TryDockBike));
        static readonly FieldInfo dockedPad = AccessTools.Field(typeof(Hoverbike), nameof(dockedPad));
        static readonly MethodInfo OnBikePickedUp = AccessTools.Method(typeof(Hoverpad), nameof(OnBikePickedUp));

        public static Hoverbike WaitingForTeleportBike = null;
        public static Hoverpad WaitingForTeleportPad = null;
        public static Vector3 WaitingPlayerPos;
        public static float WaitingTime = 0f;

        [HarmonyPatch(nameof(Update)), HarmonyPostfix]
        public static void Update(Hoverpad __instance)
        {
            var key = C.SummonKey;
            if (string.IsNullOrWhiteSpace(key))
            {
                return;
            }

            if (GameInput.GetKeyDown((KeyCode)key.ToLower()[0]))
            {
                var player = Player.main;
                if (player == null) { return; }

                var pos = __instance.transform.position;
                {
                    var distance = (player.transform.position - pos).magnitude;
                    if (distance > 5) { return; }
                }

                if (__instance.dockedBike)
                {
                    ErrorMessage.AddError("You already have a Snowfox docked");
                    return;
                }

                var bikes = UnityEngine.Object.FindObjectsOfType<Hoverbike>();
                if (bikes.Length == 0)
                {
                    ErrorMessage.AddError("You have no Snowfox yet");
                    return;
                }

                Hoverbike nearestBike = null;
                var currDist = float.MaxValue;
                foreach (var bike in bikes)
                {
                    var distance = (bike.transform.position - pos).magnitude;

                    if (distance < currDist)
                    {
                        nearestBike = bike;
                        currDist = distance;
                    }
                }

                SummonBike(nearestBike, __instance);
            }

            if (WaitingTime > 0)
            {
                WaitingTime -= Time.deltaTime;

                if (WaitingTime <= 0 && WaitingForTeleportBike != null)
                {
                    TeleportAndDockBike(WaitingForTeleportBike, WaitingForTeleportPad);
                    Player.main.transform.position = WaitingPlayerPos;

                    WaitingForTeleportBike = null;
                    WaitingForTeleportPad = null;
                }
            }
        }

        [HarmonyPatch(nameof(EndBikeUndockSequence)), HarmonyPrefix]
        public static void EndBikeUndockSequence()
        {
            if (WaitingForTeleportBike != null)
            {
                WaitingForTeleportBike.ExitVehicle();

                WaitingTime = 2f;
            }
        }

        static void SummonBike(Hoverbike bike, Hoverpad pad)
        {
            var dockingPad = dockedPad.GetValue(bike) as Hoverpad;
            if (dockingPad != null && dockingPad)
            {
                var trigger = dockingPad.GetComponent<HoverpadUndockTrigger>() ??
                    dockingPad.GetComponentInChildren<HoverpadUndockTrigger>();

                if (trigger)
                {
                    var player = Player.main;
                    if (!player) { return; }

                    WaitingForTeleportPad = pad;
                    WaitingForTeleportBike = bike;
                    WaitingPlayerPos = player.transform.position;

                    var hand = player.gameObject.GetComponent<GUIHand>();
                    trigger.OnHandClick(hand);
                }
            }
            else
            {
                TeleportAndDockBike(bike, pad);
            }
        }

        static void TeleportAndDockBike(Hoverbike bike, Hoverpad pad)
        {
            var distance = (bike.transform.position - pad.transform.position).magnitude;
            bike.energyMixin.ConsumeEnergy(C.SummonEnergyPerMeter * distance);

            var padPos = pad.transform.position;
            bike.transform.position = padPos.WithY(padPos.y + 5);

            TryDockBike.Invoke(pad, new object[] { bike });
        }

    }

}
