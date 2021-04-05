using DV;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace DvMod.HeadsUpDisplay
{
    internal static class LocoProviders
    {
        public static PushProvider tractiveEffortProvider = new PushProvider(
            "Tractive effort", f => $"{f / 1000:F0} kN");

        public static PushProvider adhesionProvider = new PushProvider(
            "Adhesion", f => $"{f / 1000:F0} kN");

        public static PushProvider indicatedPowerProvider = new PushProvider(
            "Indicated power", f => $"{f / 1000:F0} kW");

        public static void Register()
        {
            Registry.Register(tractiveEffortProvider);
            Registry.Register(adhesionProvider);
            Registry.Register(indicatedPowerProvider);
            Registry.Register(new QueryDataProvider(
                "Slip",
                car => car.GetComponent<DrivingForce>()?.wheelslip,
                f => $"{f:P1}"));

            SteamLocoProviders.Register();
        }

        [HarmonyPatch]
        public static class GetTractionForcePatch
        {
            public static void Postfix(LocoControllerBase __instance, float __result)
            {
                if (!AppUtil.IsPaused)
                {
                    tractiveEffortProvider.SetValue(__instance.train, __result);
                    indicatedPowerProvider.SetValue(__instance.train, __result * __instance.GetSpeedKmH() / 3.6f);
                }
            }

            public static IEnumerable<MethodBase> TargetMethods()
            {
                yield return AccessTools.Method(typeof(LocoControllerDiesel), nameof(LocoControllerBase.GetTractionForce));
                yield return AccessTools.Method(typeof(LocoControllerShunter), nameof(LocoControllerBase.GetTractionForce));
                yield return AccessTools.Method(typeof(LocoControllerSteam), nameof(LocoControllerBase.GetTractionForce));
            }
        }

        [HarmonyPatch(typeof(DrivingForce), "UpdateWheelslip")]
        public static class UpdateWheelslipPatch
        {
            private static readonly FieldInfo slipLimitField =
                AccessTools.DeclaredField(typeof(DrivingForce), nameof(DrivingForce.tractionForceWheelslipLimit));

            public static void Postfix(DrivingForce __instance, Bogie bogie)
            {
                var car = bogie.Car;
                adhesionProvider.SetValue(car, (float)slipLimitField.GetValue(__instance) * car.Bogies.Length);
            }
        }
    }

    internal static class SteamLocoProviders
    {
        public static PushProvider cutoffProvider = new PushProvider("Cutoff", f => $"{f:P0}");

        public static void Register()
        {
            Registry.Register(cutoffProvider);
        }

        [HarmonyPatch(typeof(SteamLocoSimulation), "SimulateTick")]
        public static class SimulateTickPatch
        {
            public static bool Prefix(SteamLocoSimulation __instance)
            {
                cutoffProvider.SetValue(TrainCar.Resolve(__instance.gameObject), __instance.cutoff.value);
                return true;
            }
        }
    }
}