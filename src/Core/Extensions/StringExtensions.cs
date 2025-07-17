using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace PrepareLanding.Core.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        ///  Repeat a string n times.
        /// </summary>
        /// <param name="s">The string to repeat.</param>
        /// <param name="n">Number of times the string must be repeated.</param>
        /// <returns>The input string repeated n times.</returns>
        public static string Repeat(this string s, int n)
        {
            return new string(Enumerable.Range(0, n).SelectMany(x => s).ToArray());
        }

        private static readonly Dictionary<string, string> TileMutatorSuffixes = new()
        {
            { "AbandonedColonyTribal", "(tribal)" },
            { "AbandonedColonyOutlander", "(outlander)" },
            { "WildTropicalPlants", "(tropical)" },
            { "LakeWithIslands", "(multiple)" },
            { "LavaCrater", "(crater)" },
            { "Cove", "(cove)" },
        };

        public static string SelectionLabel(this TileMutatorDef def)
        {
            var label = TileMutatorSuffixes.TryGetValue(def.defName, out var suffix) ? $"{def.LabelCap} {suffix}" : $"{def.LabelCap}";
            return $"<color=#999999>({def.modContentPack.Name.CapitalizeFirst()})</color> {label}";
        }

        public static string SelectionLabel(this Def def)
        {
            return $"<color=#999999>({def.modContentPack.Name.CapitalizeFirst()})</color> {def.LabelCap}";
        }
    }
}
