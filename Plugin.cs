using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UIElements;

namespace CW_FixItemSpawn
{
    [ContentWarningPlugin(ModGUID, ModVersion, vanillaCompatible: false)]
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    public class Plugin : BaseUnityPlugin
    {
        public const string ModGUID = "Electric131.FixItemSpawn";
        public const string ModName = "FixItemSpawn";
        public const string ModVersion = "1.0.0";

        public static ManualLogSource? logger;

        private void Awake()
        {
            logger = Logger;
            logger.LogInfo($"Plugin {ModGUID} loaded!");

            Harmony.CreateAndPatchAll(typeof(Plugin));

            logger.LogInfo($"Patches created successfully");
        }

        [HarmonyPatch(typeof(PersistentObjectsHolder), nameof(PersistentObjectsHolder.SpawnPersistentDiveBellObjects))]
        [HarmonyPrefix]
        public static void UpdatePatch(ref UI_Views __instance)
        {
            DivingBell divingBell = FindObjectOfType<DivingBell>();

            if (divingBell == null) return; // Game also throws an error if this is the case, so I won't throw an extra one

            Vector3 itemSpawn = divingBell.itemSpawns.position;

            Vector3 button = divingBell.transform.Find("Tools/TravelButton").position;
            Vector3 light = divingBell.transform.Find("Mesh/InLight").position;

            button.y = itemSpawn.y;
            light.y = itemSpawn.y;

            divingBell.itemSpawns.position = Vector3.Lerp(light, button, 0.5f);
        }
    }
}
