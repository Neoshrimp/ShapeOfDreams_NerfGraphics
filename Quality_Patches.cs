using GraphicsConfigurator.API.URP;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using UnityEngine;
using VolumetricFogAndMist2;

namespace SoD_NerfGraphics
{

    [HarmonyPatch(typeof(DewSave), nameof(DewSave.UpdateTerrainAndVegetationQuality))]
    class UpdateTerrainAndVegetationQuality_Patch
    {

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var matcher = new CodeMatcher(instructions);
            return matcher
                /*                //heightmap error
                                .MatchEndForward(new CodeMatch(OpCodes.Ldc_I4_S, (sbyte)100))
                                .Set(OpCodes.Ldc_I4, 100)

                                //vegetation density
                                .MatchEndForward(new CodeMatch(OpCodes.Ldc_R4, 0.25f))
                                .Set(OpCodes.Ldc_R4, 0f)*/
                .End()
                .MatchEndBackwards(new CodeMatch(OpCodes.Callvirt, AccessTools.PropertySetter(typeof(Terrain), nameof(Terrain.detailObjectDensity))))
                .Advance(1)
                .InsertAndAdvance(new CodeInstruction(OpCodes.Dup))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(UpdateTerrainAndVegetationQuality_Patch), nameof(UpdateTerrainAndVegetationQuality_Patch.TerrainSettings))))


                .InstructionEnumeration();
        }

        private static void TerrainSettings(Terrain terrain)
        {
            if (PConfig.DisableTrees)
            {
                terrain.drawTreesAndFoliage = false;
                terrain.detailObjectDensity = 0f;
            }
            else
            {
                terrain.drawTreesAndFoliage = true;
            }

            terrain.heightmapPixelError = PConfig.HeigtmapErrorTolerance;

        }
    }


    [HarmonyPatch(typeof(DewSave), nameof(DewSave.UpdateFogQuality))]
    class DisableFog_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var matcher = new CodeMatcher(instructions);
            return matcher
/*                .End()
                // downscale multiplier
                .MatchEndBackwards(new CodeMatch(OpCodes.Ldc_I4_2))
                .Set(OpCodes.Ldc_I4_8, null)*/
                

                .End()
                .MatchEndBackwards(new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(GameObject), nameof(GameObject.SetActive))))
                .Advance(1)
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_2))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(DisableFog_Patch), nameof(DisableFog_Patch.DisableFog))))
                .InstructionEnumeration();
        }

        private static void DisableFog(VolumetricFog fog)
        {
            if (PConfig.DisableVolumetricFog)
                fog.gameObject.SetActive(false);
            else
                fog.gameObject.SetActive(true);

        }
    }



    [HarmonyPatch(typeof(DewSave), nameof(DewSave.ApplyPerSceneSettings))]
    class DewSave_Patch
    {
        static void Postfix()
        {
            //Log.LogDebug("Updated graphics");

            var up = Configuring.CurrentURPA;
            up.SoftShadows(false);
            up.OpaqueTexture(false);
            up.DepthTexture(false);
            up.MaxAdditionalLightsCount(0);

        }
    }











}
