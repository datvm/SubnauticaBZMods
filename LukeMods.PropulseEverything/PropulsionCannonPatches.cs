using HarmonyLib;
using LukeMods.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LukeMods.PropulseEverything
{

    [HarmonyPatch(typeof(PropulsionCannon))]
    public class PropulsionCannonPatches
    {
        static Config C = Config.Instance;

        [HarmonyPatch(nameof(Filter)), HarmonyPrefix]
        public static bool Filter(InventoryItem item, PropulsionCannon __instance, ref bool __result)
        {
            var baseInstance = (MonoBehaviour)__instance;
            __result = !(item == null || item.item == null || baseInstance.transform.IsChildOf(item.item.GetComponent<Transform>()));

            return false;
        }

        [HarmonyPatch("TraceForGrabTarget"), HarmonyPrefix]
        public static void TraceForGrabTargetPre(PropulsionCannon __instance)
        {
            var c = C;
            __instance.maxMass = float.MaxValue;
            __instance.maxAABBVolume = float.MaxValue;
            __instance.pickupDistance = c.PickupDistance;
            __instance.attractionForce = c.AttractionForce;
            __instance.shootForce = c.ShootForce;
            __instance.massScalingFactor = c.MassScalingFactor;
        }

        [HarmonyPatch(nameof(TraceForGrabTarget)), HarmonyPostfix]
        public static void TraceForGrabTarget(PropulsionCannon __instance, ref GameObject __result)
        {
            if (__result != null)
            {
                return;
            }

            Vector3 position = MainCamera.camera.transform.position;
            int layerMask = ~(1 << LayerMask.NameToLayer("Player"));
            int num = UWE.Utils.SpherecastIntoSharedBuffer(position, 1.2f, MainCamera.camera.transform.forward, __instance.pickupDistance, layerMask);
            GameObject result = null;
            float num2 = float.PositiveInfinity;
            var checkedObjects = new HashSet<GameObject>();

            for (int i = 0; i < num; i++)
            {
                RaycastHit raycastHit = UWE.Utils.sharedHitBuffer[i];

                var layer = raycastHit.collider.gameObject.layer;
                if (raycastHit.collider.isTrigger && (layer <= 8 || layer >= 19))
                {
                    continue;
                }

                GameObject entityRoot = UWE.Utils.GetEntityRoot(raycastHit.collider.gameObject);
                if (!(entityRoot != null) || checkedObjects.Contains(entityRoot))
                {
                    continue;
                }

                float sqrMagnitude = (raycastHit.point - position).sqrMagnitude;
                if (sqrMagnitude < num2 && __instance.ValidateNewObject(entityRoot, raycastHit.point))
                {
                    result = entityRoot;
                    num2 = sqrMagnitude;

                }
                checkedObjects.Add(entityRoot);
            }

            __result = result;
        }

        [HarmonyPatch(nameof(ValidateObject)), HarmonyPrefix]
        public static bool ValidateObject(GameObject go, PropulsionCannon __instance, ref bool __result)
        {
            if (!go.activeSelf || !go.activeInHierarchy)
            {
                __result = false;
                Debug.Log("object is inactive");
                return false;
            }

            __result = go.GetComponent<Rigidbody>() != null;
            return false;
        }

        [HarmonyPatch(nameof(ValidateNewObject)), HarmonyPrefix]
        public static bool ValidateNewObject(GameObject go, PropulsionCannon __instance, ref bool __result)
        {
            bool result = false;
            ValidateObject(go, __instance, ref result);

            __result = result;
            return false;
        }

        [HarmonyPatch(nameof(ReleaseGrabbedObject)), HarmonyPrefix]
        public static bool ReleaseGrabbedObject(PropulsionCannon __instance)
        {
            if (__instance.grabbedObject != null)
            {
                PropulseCannonAmmoHandler component = __instance.grabbedObject.GetComponent<PropulseCannonAmmoHandler>();

                if (component != null)
                {
                    component.UndoChanges();
                    UnityEngine.Object.Destroy(component);
                }

                __instance.grabbedObject = null;
            }

            return false;
        }

        [HarmonyPatch(nameof(Update)), HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> Update(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldc_R4 && instruction.operand.Equals(.7f))
                {
                    yield return new CodeInstruction(OpCodes.Ldc_R4, C.EnergyRate * .7f);
                }
                else
                {
                    yield return instruction;
                }
            }
        }

    }

}
