using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SoD_NerfGraphics
{
    public static class PConfig
    {

        public enum ReduceSelfEffects
        {
            Off,
            Low,
            Medium,
            High,
            VeryHigh,
            Hide
        }

        public static bool MapSelfEffectsToOther(this ReduceSelfEffects selfE, out ReduceOtherPlayerEffectsStrength otherE)
        {
            otherE = ReduceOtherPlayerEffectsStrength.Low;
            switch (selfE)
            {
                case ReduceSelfEffects.Off:
                    return false;
                case ReduceSelfEffects.Low:
                    otherE = ReduceOtherPlayerEffectsStrength.Low;
                    break;
                case ReduceSelfEffects.Medium:
                    otherE = ReduceOtherPlayerEffectsStrength.Medium;
                    break;
                case ReduceSelfEffects.High:
                    otherE = ReduceOtherPlayerEffectsStrength.High;
                    break;
                case ReduceSelfEffects.VeryHigh:
                    otherE = ReduceOtherPlayerEffectsStrength.VeryHigh;
                    break;
                case ReduceSelfEffects.Hide:
                    otherE = ReduceOtherPlayerEffectsStrength.Hide;
                    break;
                default:
                    return false;
            }
            return true;
        }

        private static string graphicsSection = "Graphics";

        public static void DoBinds(ConfigFile configFile)
        {
            //selfEffectReductionLevel = configFile.Bind(graphicsSection, "selfEffectReductionLevel", ReduceSelfEffects.High, "Suppress vfx of self skills. Higher level => more muted effects. Decent performance increase, especially in later worlds.");

            disableVolumetricFog = configFile.Bind(graphicsSection, "disableVolumetricFog", true, "Disable foreground filter and godrays. Moderate performance increase.");

            disableTrees = configFile.Bind(graphicsSection, "disableTrees", true, "Disable background objects and vegetation. Massive performance increase.");

            heigtmapErrorTolerance = configFile.Bind(graphicsSection, "heigtmapErrorTolerance", 100, "Makes the floor 'flatter'. High values cause noticeable visual distortion and character clipping. Variable performance increase. Default value is vanilla's lowest setting.");

            toggleDodgeToCursor = configFile.Bind("Controls", "toggleDodgeToCursor", KeyCode.None, "Toggle directional/dodge to cursor. For Primus fireballs.");



        }

        static ConfigEntry<ReduceSelfEffects> selfEffectReductionLevel;

        static ConfigEntry<bool> disableVolumetricFog;

        static ConfigEntry<bool> disableTrees;

        static ConfigEntry<int> heigtmapErrorTolerance;

        static ConfigEntry<KeyCode> toggleDodgeToCursor;

        public static ReduceSelfEffects ReduceSelfEffect { get => selfEffectReductionLevel.Value; }
        public static bool DisableVolumetricFog { get => disableVolumetricFog.Value; }
        public static bool DisableTrees { get => disableTrees.Value; }
        public static int HeigtmapErrorTolerance { get => heigtmapErrorTolerance.Value; }
        public static KeyCode ToggleDodgeToCursor { get => toggleDodgeToCursor.Value; }
    }
}
