using HarmonyLib;
using Verse;

namespace PrepareLanding.Patches
{
    [HarmonyPatch(typeof(Game), nameof(Game.FinalizeInit))]
    public static class PatchGameFinalizeInit
    {
        [HarmonyPostfix]
        public static void GameFinalizeInitPostFix()
        {
            PrepareLanding.Instance?.WorldLoaded();
        }
    }
}
