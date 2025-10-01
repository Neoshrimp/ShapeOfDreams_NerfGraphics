using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoD_NerfGraphics
{

    [HarmonyPatch(typeof(ElementalStatusEffect), "Awake")]
    class DisableParticleSystems_Patch
    {
        static void Prefix(ElementalStatusEffect __instance)
        {
            if (__instance.stackScaledEmissions == null)
                return;
            if (__instance.stackScaledEmissions.Length <= 1)
                return;

            var ems = __instance.stackScaledEmissions;

            for (int i = 1; i < ems.Length; i++)
            {
                __instance.stackScaledEmissions[i].Stop(true);
            }



        }
    }


}
