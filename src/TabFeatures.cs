using PrepareLanding.Core.Extensions;
using PrepareLanding.Core.Gui.Tab;
using PrepareLanding.Filters;
using RimWorld;
using UnityEngine;
using Verse;
using Widgets = PrepareLanding.Core.Gui.Widgets;

namespace PrepareLanding
{
    public class TabFeatures : TabGuiUtility
    {
        private static Vector2 _scrollPosFeatureSelection = Vector2.zero;
        private static Vector2 _scrollPosAdjBiomeSelection = Vector2.zero;

        private readonly GameData.GameData _gameData;

        public TabFeatures(GameData.GameData gameData, float columnSizePercent = 0.48f) : base(columnSizePercent)
        {
            _gameData = gameData;
        }

        public override bool CanBeDrawn { get; set; } = true;

        public override string Id => "Features";

        public override string Name => "PLMWTF_TabName".Translate();

        public override void Draw(Rect inRect)
        {
            Begin(inRect);
            DrawFeatureSelection();
            NewColumn();
            DrawAdjBiomeSelection();
            End();
        }

        private void DrawFeatureSelection()
        {
            DrawEntryHeader("PLMWTF_Features".Translate(), backgroundColor: ColorFromFilterType(typeof(TileFilterFeatures)));

            var defs = _gameData.DefData.TileMutatorDefs;
            var container = _gameData.UserData.SelectedTileMutatorDefs;

            var numButtons = 4;
            if (_gameData.UserData.Options.ViewPartialOffNoSelect)
                numButtons += 1;

            var buttonsRect = ListingStandard.GetRect(DefaultElementHeight).SplitRectWidthEvenly(numButtons);
            if (buttonsRect.Count != numButtons)
            {
                Log.ErrorOnce($"[PrepareLanding] DrawFeatureSelection: couldn't get the right number of buttons: {numButtons}", 0x1239cafe);
                return;
            }

            if (Widgets.ButtonTextToolTip(buttonsRect[0], "PLMW_Reset".Translate(), "PLMWTT_ButtonResetTooltip".Translate()))
                container.Reset(defs, nameof(_gameData.UserData.SelectedTileMutatorDefs));

            if (Widgets.ButtonTextToolTip(buttonsRect[1], "PLMW_All".Translate(), "PLMWTT_ButtonAllTooltip".Translate()))
                container.All();

            if (Widgets.ButtonTextToolTip(buttonsRect[2], "PLMW_None".Translate(), "PLMWTT_ButtonNoneTooltip".Translate()))
                container.None();

            if (Widgets.ButtonTextToolTipColor(buttonsRect[3], container.FilterBooleanState.ToStringHuman(), "PLMWTT_ORANDTooltip".Translate(), container.FilterBooleanState.Color()))
            {
                container.FilterBooleanState = container.FilterBooleanState.Next();
            }

            if (_gameData.UserData.Options.ViewPartialOffNoSelect)
            {
                var color = container.OffPartialNoSelect ? Color.green : Color.red;
                if (Widgets.ButtonTextToolTipColor(buttonsRect[4], $"{"PLMWTT_SelectedShort".Translate()} {container.OffPartialNoSelect}", "PLMWTT_OffPartialTooltip".Translate(), color))
                {
                    container.OffPartialNoSelect = !container.OffPartialNoSelect;
                }
            }

            var scrollViewHeight = container.Count * DefaultElementHeight;
            var inLs = ListingStandard.BeginScrollView(15 * DefaultElementHeight, scrollViewHeight,
                ref _scrollPosFeatureSelection, DefaultScrollableViewShrinkWidth);

            foreach (var def in defs)
            {
                if (!container.TryGetValue(def, out var threeStateItem))
                {
                    Log.Error($"[PrepareLanding] [DrawFeatureSelection] an item in TileMutatorDefs is not in SelectedTileMutatorDefs: {def.LabelCap}");
                    continue;
                }

                var tmpState = threeStateItem.State;

                var disabled = IsFeatureDisabled(def);
                var itemRect = inLs.GetRect(DefaultElementHeight);
                var label = def.SelectionLabel();

                if (disabled)
                    label = $"<color=#666666>{label} (disabled)</color>";

                Widgets.CheckBoxLabeledMulti(itemRect, label, ref tmpState, disabled);

                if (disabled)
                    tmpState = MultiCheckboxState.Partial;

                if (tmpState != threeStateItem.State)
                    threeStateItem.State = tmpState;

                if (!string.IsNullOrEmpty(def.description))
                    TooltipHandler.TipRegion(itemRect, def.description);
            }

            ListingStandard.EndScrollView(inLs);
        }

        /// <summary>
        /// Extension point for mods to patch via Harmony.
        /// </summary>
        public static bool IsFeatureDisabled(TileMutatorDef def)
        {
            return false;
        }

        private void DrawAdjBiomeSelection()
        {
            DrawEntryHeader("PLMWTF_AdjBiomes".Translate(), backgroundColor: ColorFromFilterType(typeof(TileFilterAdjBiomes)));

            var defs = _gameData.DefData.BiomeDefs;
            var container = _gameData.UserData.SelectedAdjBiomeDefs;

            var numButtons = 4;
            if (_gameData.UserData.Options.ViewPartialOffNoSelect)
                numButtons += 1;

            var buttonsRect = ListingStandard.GetRect(DefaultElementHeight).SplitRectWidthEvenly(numButtons);
            if (buttonsRect.Count != numButtons)
            {
                Log.ErrorOnce($"[PrepareLanding] DrawAdjBiomeSelection: couldn't get the right number of buttons: {numButtons}", 0x1239cafe);
                return;
            }

            if (Widgets.ButtonTextToolTip(buttonsRect[0], "PLMW_Reset".Translate(), "PLMWTT_ButtonResetTooltip".Translate()))
                container.Reset(defs, nameof(_gameData.UserData.SelectedAdjBiomeDefs));

            if (Widgets.ButtonTextToolTip(buttonsRect[1], "PLMW_All".Translate(), "PLMWTT_ButtonAllTooltip".Translate()))
                container.All();

            if (Widgets.ButtonTextToolTip(buttonsRect[2], "PLMW_None".Translate(), "PLMWTT_ButtonNoneTooltip".Translate()))
                container.None();

            if (Widgets.ButtonTextToolTipColor(buttonsRect[3], container.FilterBooleanState.ToStringHuman(), "PLMWTT_ORANDTooltip".Translate(), container.FilterBooleanState.Color()))
            {
                container.FilterBooleanState = container.FilterBooleanState.Next();
            }

            if (_gameData.UserData.Options.ViewPartialOffNoSelect)
            {
                var color = container.OffPartialNoSelect ? Color.green : Color.red;
                if (Widgets.ButtonTextToolTipColor(buttonsRect[4], $"{"PLMWTT_SelectedShort".Translate()} {container.OffPartialNoSelect}", "PLMWTT_OffPartialTooltip".Translate(), color))
                {
                    container.OffPartialNoSelect = !container.OffPartialNoSelect;
                }
            }

            var scrollViewHeight = container.Count * DefaultElementHeight;
            var inLs = ListingStandard.BeginScrollView(15 * DefaultElementHeight, scrollViewHeight,
                ref _scrollPosAdjBiomeSelection, DefaultScrollableViewShrinkWidth);

            foreach (var def in defs)
            {
                if (!container.TryGetValue(def, out var threeStateItem))
                {
                    Log.Error($"[PrepareLanding] [DrawAdjBiomeSelection] an item in BiomeDefs is not in SelectedAdjBiomeDefs: {def.LabelCap}");
                    continue;
                }

                var tmpState = threeStateItem.State;

                var itemRect = inLs.GetRect(DefaultElementHeight);
                Widgets.CheckBoxLabeledMulti(itemRect, def.SelectionLabel(), ref tmpState);

                if (tmpState != threeStateItem.State)
                    threeStateItem.State = tmpState;

                if (!string.IsNullOrEmpty(def.description))
                    TooltipHandler.TipRegion(itemRect, def.description);
            }

            ListingStandard.EndScrollView(inLs);
        }
    }
}
