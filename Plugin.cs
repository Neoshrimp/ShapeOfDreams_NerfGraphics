using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;


namespace SoD_NerfGraphics
{
    [BepInPlugin(PInfo.GUID, PInfo.name, PInfo.version)]
    [BepInProcess("Shape of Dreams.exe")]
    public class Plugin : BaseUnityPlugin
    {
        
        private static readonly Harmony harmony = new Harmony(PInfo.GUID);

        internal static BepInEx.Logging.ManualLogSource log;

        private void Awake()
        {
            log = Logger;

            // very important(maybe). Without this the entry point MonoBehaviour gets destroyed
            DontDestroyOnLoad(gameObject);
            gameObject.hideFlags = HideFlags.HideAndDontSave;

            PConfig.DoBinds(Config);

            harmony.PatchAll();

        }

        private void OnDestroy()
        {
            if (harmony != null)
                harmony.UnpatchSelf();
        }


        private void Update()
        {
            
            if (Input.GetKeyDown(PConfig.ToggleDodgeToCursor) && DewSave.profileMain != null) 
            {
                if (DewSave.profileMain.controls.dashDirectionWhenDirectionalMovement == DashDirection.CurrentMovement)
                {
                    DewSave.profileMain.controls.dashDirectionWhenDirectionalMovement = DashDirection.AllTowardsCursor;
                    Log.LogDebug("dash towards cursor");
                }
                else
                {
                    DewSave.profileMain.controls.dashDirectionWhenDirectionalMovement = DashDirection.CurrentMovement;
                    Log.LogDebug("dash directional");
                }
            }
        }

    }
}
