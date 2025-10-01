using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using Object = UnityEngine.Object;

namespace SoD_NerfGraphics
{
    //[HarmonyPatch(typeof(DewResources), nameof(DewResources.QualityAdjustedProcessor))]
    class AttachEffectObject_Patch
    {

        //public static HashSet<Object> playerObjects = new HashSet<Object>();

        //[ThreadStatic]
        public static bool isPlayerEffect;

        static bool Prefix(Object o)
        {

            if (PConfig.ReduceSelfEffect == PConfig.ReduceSelfEffects.Off)
                return true;

            //playerObjects.Add(o);
            isPlayerEffect = true;
            DewResources.TonedDownProcessor(o);
            isPlayerEffect = false;
            //playerObjects.Remove(o);

            return false;
        }
        
        /*static void Postfix()
        {
            isPlayerEffect = false;

        }*/

    }

    //[HarmonyPatch(typeof(DewResources), nameof(DewResources.TonedDownProcessor))]
    class TonedDownProcessor_Patch
    {

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var matcher = new CodeMatcher(instructions);
            return matcher
                .MatchEndForward(new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(DewGameplaySettings_User), nameof(DewGameplaySettings_User.reduceOtherPlayerEffectsStrength))))
                .Advance(1)
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_0))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(TonedDownProcessor_Patch), nameof(TonedDownProcessor_Patch.SortByEffectSource))))
                .InstructionEnumeration();
        }

        private static ReduceOtherPlayerEffectsStrength SortByEffectSource(ReduceOtherPlayerEffectsStrength ogConfig, Object o)
        {
            //if (AttachEffectObject_Patch.playerObjects.Contains(o))
            if (AttachEffectObject_Patch.isPlayerEffect)
            {
                //Log.LogDebug($"Reducing: {o.name}");

                PConfig.ReduceSelfEffect.MapSelfEffectsToOther(out var reduceOtherPlayerEffectsStrength);
                return reduceOtherPlayerEffectsStrength;
            }
            //Log.LogWarning($"Reducing other player: {o.name}");
            return ogConfig;
        }
    }

    //[HarmonyPatch(typeof(DewResources), nameof(DewResources.GetVariant))]
    class DewResources_Patch
    {
        static void Prefix(string guid, UnityEngine.Object obj, VariantDef varDef)
        {
            Log.LogInfo($"{guid};{obj?.name};{varDef.id0},{varDef.id1},{varDef.id2},{varDef.id3}");

        }
    }


}
