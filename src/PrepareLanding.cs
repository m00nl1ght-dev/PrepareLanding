using HarmonyLib;
using PrepareLanding.Core;
using PrepareLanding.Core.Gui.World;
using PrepareLanding.Presets;
using UnityEngine;
using Verse;

namespace PrepareLanding
{
    /// <summary>
    ///     Main Mod class.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PrepareLanding : Mod
    {
        /// <summary>
        ///     Main mod class constructor. Sets up the static instance.
        /// </summary>
        public PrepareLanding(ModContentPack mcp) : base(mcp)
        {
            if (Instance == null)
                Instance = this;

            // initialize events
            EventHandler = new RimWorldEventHandler();

            // global game options
            GameOptions = GetSettings<GameOptions>();

            // instance used to keep track of (or override) game ticks.
            GameTicks = new GameTicks();

            // Holds various mod options (shown on the 'option' tab on the GUI).
            var filterOptions = new FilterOptions();

            GameData = new GameData.GameData(filterOptions);

            TileFilter = new WorldTileFilter(GameData.UserData);

            // instantiate the tile highlighter
            TileHighlighter = new TileHighlighter(filterOptions);

            var harmony = new Harmony("com.neitsa.preparelanding");
            harmony.PatchAll(typeof(PrepareLanding).Assembly);
        }

        /// <summary>
        ///     Main static instance, holding references to useful class instances.
        /// </summary>
        public static PrepareLanding Instance { get; private set; }


        public GameOptions GameOptions { get; }

        /// <summary>
        ///     Instance used to keep track of (or override) game ticks.
        /// </summary>
        public GameTicks GameTicks { get; }

        /// <summary>
        ///     The filtering class instance used to filter tiles on the world map.
        /// </summary>
        public WorldTileFilter TileFilter { get; }

        /// <summary>
        ///     Allow highlighting filtered tiles on the world map.
        /// </summary>
        public TileHighlighter TileHighlighter { get; }

        /// <summary>
        ///     Holds various game data: some are game, user, or world specific.
        /// </summary>
        public GameData.GameData GameData { get; }

        /// <summary>
        ///     The main GUI window instance.
        /// </summary>
        public MainWindow MainWindow { get; set; }

        /// <summary>
        ///     The full path of the mod folder.
        /// </summary>
        public string ModFolder => Content.RootDir;

        /// <summary>
        ///      Instance used to control all useful events from RimWorld.
        /// </summary>
        public readonly RimWorldEventHandler EventHandler;

        /// <summary>
        ///     Set the main instance to null.
        /// </summary>
        public static void RemoveInstance()
        {
            Instance = null;
        }

        internal void Initialize()
        {
            // alert subscribers that definitions have been loaded.
            EventHandler.OnDefsLoaded();

            var presetManager = new PresetManager(GameData);
            GameData.PresetManager = presetManager;
        }

        internal void WorldLoaded()
        {
            Log.Message("[PrepareLanding] WorldLoaded (from save).");

            EventHandler.OnWorldLoaded();
        }

        public override void DoSettingsWindowContents(Rect rect)
        {
            GameOptions.DoSettingsWindowContents(rect);
        }

        public override string SettingsCategory() => "Prepare Landing";
    }
}
