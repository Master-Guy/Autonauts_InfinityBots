using Harmony;
using System;
using System.Reflection;
using UnityModManagerNet;

namespace InfiniteBots
{
    // captncraig, thanks for the initials code ;-)
    public static class Main
    {
        public static UnityModManager.ModEntry mod;
        
        private static bool Load(UnityModManager.ModEntry modEntry)
        {
            mod = modEntry;
            var harmony = HarmonyInstance.Create(modEntry.Info.Id);

            harmony.Patch(
                original: AccessTools.Method(typeof(Worker),"InitStats"),
                postfix: new HarmonyMethod(typeof(Main), nameof(Main.InitStats))
            );

            Type type = typeof(Worker);
            FieldInfo info = type.GetField("m_LoseEnergy", BindingFlags.NonPublic | BindingFlags.Static);
            info.SetValue(null, false);

            return true;
        }

        [HarmonyPostfix]
        static void InitStats()
        {
            Worker.m_AllHeadInfo.ForEach(delegate (WorkerHeadInfo whi)
            {
                whi.m_MaxInstructions = Int32.MaxValue;
                whi.m_SerialPrefix = "IB";
            });
        }

    }
}
