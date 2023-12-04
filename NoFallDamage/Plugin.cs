using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Linq;

namespace NoFallDamage
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class NoFallDamagePatch : BaseUnityPlugin, IPatchable
    {
        private const string modGUID = "Billy.exe__NoFallDamageMod";
        private const string modName = "NoFallDamageMod";
        private const string modVersion = "1.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);
        private static NoFallDamagePatch Instance;

        public static ManualLogSource mls;

        void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);

            //cycle through the whole project and add all the classes that implements 
            //IPatchable interface to the harmony patches
            var interfaceType = typeof(IPatchable);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => interfaceType.IsAssignableFrom(p) && !p.IsInterface);

            mls.LogInfo("NoFallDamage loading");
            foreach(var type in types)
            {
                mls.LogInfo($"loading {type.Name}");
                harmony.PatchAll(type);
            }
            mls.LogInfo("NoFallDamage fully loaded");
        }
    }
}
