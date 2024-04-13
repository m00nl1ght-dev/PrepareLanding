using UnityEngine;
using Verse;

namespace PrepareLanding
{
    public class GameOptions : ModSettings
    {
        public bool DisableWorldData;

        public bool DisablePreciseWorldGenPercentage = true;

        public KeyCode PrepareLandingHotKey = KeyCode.P;

        public void DoSettingsWindowContents(Rect rect)
        {
            var listingStandard = new Listing_Standard();
            listingStandard.Begin(rect);

            listingStandard.CheckboxLabeled("PLGOPT_DisableWorldDataTitle".Translate(), ref DisableWorldData,
                "PLGOPT_DisableWorldDataDescription".Translate());

            listingStandard.CheckboxLabeled("Disable Precise World Gen. %", ref DisablePreciseWorldGenPercentage,
                "Disable Precise World Generation Percentage on the Create World parameter page.");

            listingStandard.End();
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref DisableWorldData, "DisableWorldData");
            Scribe_Values.Look(ref DisablePreciseWorldGenPercentage, "DisablePreciseWorldGenPercentage", true);
            Scribe_Values.Look(ref PrepareLandingHotKey, "PrepareLandingHotKey", KeyCode.P);
            base.ExposeData();
        }
    }
}
