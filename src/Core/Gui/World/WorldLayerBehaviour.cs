using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace PrepareLanding.Core.Gui.World
{
    [StaticConstructorOnStartup]
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class WorldLayerBehaviour : MonoBehaviour
    {
        private const string GameObjectName = "PrepareLandingWorldLayerBehaviour";

        public static WorldLayerBehaviour Instance { get; private set; }

        /// <summary>
        ///     Default material for highlighted tiles.
        /// </summary>
        public static Material DefaultTileHighlighterMaterial { get; private set; }

        /// <summary>
        ///     Default tile highlighting color.
        /// </summary>
        public static Color MaterialColor = Color.green;

        public bool Enabled => enabled;

        /// <summary>
        ///     Get or set the highlighted tile color.
        /// </summary>
        public Color TileColor
        {
            get => MaterialColor;
            set
            {
                MaterialColor = value;
                DefaultTileHighlighterMaterial.color = MaterialColor;
            }
        }

        static WorldLayerBehaviour()
        {
            #if DEBUG
            Log.Message("[PrepareLanding] WorldLayerBehaviour Static Initialization");
            #endif

            var gameObject = new GameObject(GameObjectName);

            DontDestroyOnLoad(gameObject);

            Instance = gameObject.AddComponent<WorldLayerBehaviour>();
        }

        public virtual void Start()
        {
            #if DEBUG
            Log.Message("[PrepareLanding] WorldLayerBehaviour Start");
            #endif

            // do not enable Update();
            // see https://docs.unity3d.com/ScriptReference/Behaviour-enabled.html
            enabled = false;

            // setup default material for the tile highlighter.
            DefaultTileHighlighterMaterial =
                new Material(WorldMaterials.SelectedTile) {color = MaterialColor};

            // setup alpha color value.
            var matColor = DefaultTileHighlighterMaterial.color;
            matColor.a = 1.0f;
            DefaultTileHighlighterMaterial.color = matColor;
        }

        public void OnDestroy()
        {
            var currentGameObject = GameObject.Find(GameObjectName);
            Destroy(currentGameObject);

            Instance = null;

            #if DEBUG
            Log.Message("[PrepareLanding] WorldLayerBehaviour OnDestroy");
            #endif
        }
    }
}
