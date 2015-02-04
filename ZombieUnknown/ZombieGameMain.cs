using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Engine;
using Engine.AI.BehaviorTrees;
using Engine.Drawing;
using Engine.Entities;
using Engine.Maps;
using Engine.Serialization;
using Engine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZombieUnknown.AI;
using ZombieUnknown.AI.BehaviorTrees;
using ZombieUnknown.Entities;
using Console = Engine.Drawing.Console;

namespace ZombieUnknown
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ZombieGameMain : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private Map _map;
        private PathfindingMap _pathfindingMap;
        private LightMap _lightMap;
        private ICamera _camera;

        private VirtualScreen _virtualScreen;
        private DrawingManager _drawingManager;
        private UIManager _uiManager;

        private Vector2 _mapSize = new Vector2(20, 20);
        private SpriteBatch _spriteBatch;

        public ZombieGameMain()
        {
            _graphics = new GraphicsDeviceManager(this)
                {
                    
                    PreferredBackBufferWidth = 1920,
                    PreferredBackBufferHeight = 1080,
                    IsFullScreen = false
                };
            Content.RootDirectory = "Content";

#if WINDOWS
            //Window.IsBorderless = true;
#endif

            _graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _virtualScreen = new VirtualScreen(640, 360, GraphicsDevice);

#if WINDOWS
            Window.ClientSizeChanged += Window_ClientSizeChanged;
            Window.AllowUserResizing = true;
#endif

            _camera = new Camera(new Vector2(_virtualScreen.VirtualWidth, _virtualScreen.VirtualHeight), 100, new IsometricConfiguration());
            _drawingManager = new DrawingManager(_camera);
            _uiManager = new UIManager();

            GameState.GraphicsDevice = GraphicsDevice;
            GameState.GameTime = new GameTime();

            base.Initialize();
        }

        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            _virtualScreen.PhysicalResolutionChanged();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            BehaviorTreeStore.Generate();

            // All of the initialization stuff should be somewhere else, and probably load from data files
            var fontSpriteSheet = SpriteSheetLoader.FromPath("Content/Fonts/dbmf_4x5_box");
            var terrainSpriteSheet = SpriteSheetLoader.FromPath("Content/SpriteSheets/xcom-forest");
            var wallSpriteSheet = SpriteSheetLoader.FromPath("Content/SpriteSheets/walls");
            var itemsSpriteSheet = SpriteSheetLoader.FromPath("Content/SpriteSheets/items");

            var font = new Font(fontSpriteSheet);
            
            var leftWallSprite = new StaticSprite("left", wallSpriteSheet, new Vector2(16, 40), new BoundingBox(new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0.9f, 0.1f, 0.9f)), "left");
            var rightWallSprite = new StaticSprite("right", wallSpriteSheet, new Vector2(16, 40), new BoundingBox(new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0.1f, 0.9f, 0.9f)), "right");
            var internalJoinWallSprite = new StaticSprite("internalJoin", wallSpriteSheet, new Vector2(16, 40), new BoundingBox(new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0.2f, 0.2f, 0.9f)), "join");
            var externalJoinWallSprite = new StaticSprite("externalJoin", wallSpriteSheet, new Vector2(16, 41), new BoundingBox(new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0.2f, 0.2f, 0.9f)), "join");

            var humanSprite = BuildHumanSprite();
            var zombieSprite = BuildZombieSprite();
            var foodSprite = new StaticSprite("food", itemsSpriteSheet, new Vector2(16, 32), new BoundingBox(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.9f, 0.9f, 0.5f)), "food");

            var terrainSprites = new List<Sprite>
            {
                new StaticSprite("grass", terrainSpriteSheet, new Vector2(16, 32), new BoundingBox(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.9f, 0.9f, 0.0f)), "grass"),
                new StaticSprite("grass", terrainSpriteSheet, new Vector2(16, 32), new BoundingBox(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.9f, 0.9f, 0.0f)), "tallGrass1"),
                new StaticSprite("grass", terrainSpriteSheet, new Vector2(16, 32), new BoundingBox(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.9f, 0.9f, 0.0f)), "tallGrass2")
            };

            var tiles = new Tile[(int)_mapSize.X, (int)_mapSize.Y];
            for (var y = 0; y < (int) _mapSize.Y; y++)
            {
                for (var x = 0; x < (int)_mapSize.X; x++)
                {
                    tiles[x, y] = new Tile(new Vector2(x, y), terrainSprites[0], null, null, null);
                }
            }

            tiles[3, 4].SetLeftWall(leftWallSprite);
            tiles[3, 4].SetRightWall(rightWallSprite);
            tiles[3, 4].SetJoinWall(internalJoinWallSprite);
            tiles[3, 5].SetRightWall(rightWallSprite);
            tiles[3, 6].SetRightWall(rightWallSprite);
            tiles[3, 7].SetLeftWall(leftWallSprite);
            tiles[4, 4].SetLeftWall(leftWallSprite);
            tiles[5, 4].SetLeftWall(leftWallSprite);
            tiles[5, 7].SetLeftWall(leftWallSprite);
            tiles[6, 4].SetRightWall(rightWallSprite);
            tiles[6, 6].SetRightWall(rightWallSprite);
            tiles[6, 7].SetJoinWall(externalJoinWallSprite);

            _map = new Map((short)_mapSize.X, (short)_mapSize.Y, tiles);
            GameState.Map = _map;

            _pathfindingMap = new PathfindingMap();
            GameState.PathfindingMap = _pathfindingMap;
            
            var light = new PhantomLight("light", new Coordinate(12, 5), new Light(new Coordinate(2, 1), Color.Blue, 10));
            _map.AddEntity(light);
            var light2 = new PhantomLight("light2", new Coordinate(4, 5), new Light(new Coordinate(4, 5), Color.White, 10));
            _map.AddEntity(light2);

            _lightMap = new LightMap(_map, new Color(0.15f, 0.15f, 0.25f));

            var human = new Human("human", humanSprite, new Coordinate(6, 5));
            _map.AddEntity(human);

            var rand = new Random();
            for (var i = 0; i < 999; i++)
            {
                var newLocationX = rand.Next(GameState.Map.Width);
                var newLocationY = rand.Next(GameState.Map.Height);
                var h = new Human("human" + i, humanSprite, new Coordinate(newLocationX, newLocationY));
                _map.AddEntity(h);
            }

            var food = new Food("food", foodSprite, new Coordinate(3, 4));
            _map.AddEntity(food);
            _pathfindingMap.AddBlockage(food);

            Console.Initialize(_spriteBatch, font, 10);
            Console.WriteLine("THE CONSOLE");
            FrameRater.Initialize(_spriteBatch, font);

            _drawingManager.RegisterProvider(_map);
            //_drawingManager.RegisterProvider(_pathfindingMap);
            _uiManager.RegisterProvider(Console.DrawingProvider);
            _uiManager.RegisterProvider(FrameRater.DrawingProvider);

            GameState.RandomNumberGenerator = new Random();
            GameState.MainCharacter = human;
        }

        private AnimatedSprite BuildHumanSprite()
        {
            var humanSpriteSheet = SpriteSheetLoader.FromPath("Content/SpriteSheets/civf");

            var humanAnimationList = new AnimationList();
            var idleSouthEast = new Animation(AnimationType.RunOnce);
            idleSouthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("idleSouthEast"), 1.0));
            var idleSouth = new Animation(AnimationType.RunOnce);
            idleSouth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("idleSouth"), 1.0));
            var idleSouthWest = new Animation(AnimationType.RunOnce);
            idleSouthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("idleSouthWest"), 1.0));
            var idleWest = new Animation(AnimationType.RunOnce);
            idleWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("idleWest"), 1.0));
            var idleNorthWest = new Animation(AnimationType.RunOnce);
            idleNorthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("idleNorthWest"), 1.0));
            var idleNorth = new Animation(AnimationType.RunOnce);
            idleNorth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("idleNorth"), 1.0));
            var idleNorthEast = new Animation(AnimationType.RunOnce);
            idleNorthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("idleNorthEast"), 1.0));
            var idleEast = new Animation(AnimationType.RunOnce);
            idleEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("idleEast"), 1.0));

            var walkNorth = new Animation(AnimationType.Looped);
            walkNorth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorth0"), 0.15));
            walkNorth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorth1"), 0.15));
            walkNorth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorth2"), 0.15));
            walkNorth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorth3"), 0.15));
            walkNorth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorth4"), 0.15));
            walkNorth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorth5"), 0.15));
            walkNorth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorth6"), 0.15));
            walkNorth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorth7"), 0.15));

            var walkNorthEast = new Animation(AnimationType.Looped);
            walkNorthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthEast0"), 0.15));
            walkNorthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthEast1"), 0.15));
            walkNorthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthEast2"), 0.15));
            walkNorthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthEast3"), 0.15));
            walkNorthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthEast4"), 0.15));
            walkNorthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthEast5"), 0.15));
            walkNorthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthEast6"), 0.15));
            walkNorthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthEast7"), 0.15));

            var walkEast = new Animation(AnimationType.Looped);
            walkEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkEast0"), 0.15));
            walkEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkEast1"), 0.15));
            walkEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkEast2"), 0.15));
            walkEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkEast3"), 0.15));
            walkEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkEast4"), 0.15));
            walkEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkEast5"), 0.15));
            walkEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkEast6"), 0.15));
            walkEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkEast7"), 0.15));

            var walkSouthEast = new Animation(AnimationType.Looped);
            walkSouthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthEast0"), 0.15));
            walkSouthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthEast1"), 0.15));
            walkSouthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthEast2"), 0.15));
            walkSouthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthEast3"), 0.15));
            walkSouthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthEast4"), 0.15));
            walkSouthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthEast5"), 0.15));
            walkSouthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthEast6"), 0.15));
            walkSouthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthEast7"), 0.15));

            var walkSouth = new Animation(AnimationType.Looped);
            walkSouth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouth0"), 0.15));
            walkSouth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouth1"), 0.15));
            walkSouth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouth2"), 0.15));
            walkSouth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouth3"), 0.15));
            walkSouth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouth4"), 0.15));
            walkSouth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouth5"), 0.15));
            walkSouth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouth6"), 0.15));
            walkSouth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouth7"), 0.15));

            var walkSouthWest = new Animation(AnimationType.Looped);
            walkSouthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthWest0"), 0.15));
            walkSouthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthWest1"), 0.15));
            walkSouthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthWest2"), 0.15));
            walkSouthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthWest3"), 0.15));
            walkSouthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthWest4"), 0.15));
            walkSouthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthWest5"), 0.15));
            walkSouthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthWest6"), 0.15));
            walkSouthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthWest7"), 0.15));

            var walkWest = new Animation(AnimationType.Looped);
            walkWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkWest0"), 0.15));
            walkWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkWest1"), 0.15));
            walkWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkWest2"), 0.15));
            walkWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkWest3"), 0.15));
            walkWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkWest4"), 0.15));
            walkWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkWest5"), 0.15));
            walkWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkWest6"), 0.15));
            walkWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkWest7"), 0.15));

            var walkNorthWest = new Animation(AnimationType.Looped);
            walkNorthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthWest0"), 0.15));
            walkNorthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthWest1"), 0.15));
            walkNorthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthWest2"), 0.15));
            walkNorthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthWest3"), 0.15));
            walkNorthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthWest4"), 0.15));
            walkNorthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthWest5"), 0.15));
            walkNorthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthWest6"), 0.15));
            walkNorthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthWest7"), 0.15));

            var runNorth = new Animation(AnimationType.Looped);
            runNorth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorth0"), 0.06));
            runNorth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorth1"), 0.06));
            runNorth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorth2"), 0.06));
            runNorth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorth3"), 0.06));
            runNorth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorth4"), 0.06));
            runNorth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorth5"), 0.06));
            runNorth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorth6"), 0.06));
            runNorth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorth7"), 0.06));

            var runNorthEast = new Animation(AnimationType.Looped);
            runNorthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthEast0"), 0.06));
            runNorthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthEast1"), 0.06));
            runNorthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthEast2"), 0.06));
            runNorthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthEast3"), 0.06));
            runNorthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthEast4"), 0.06));
            runNorthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthEast5"), 0.06));
            runNorthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthEast6"), 0.06));
            runNorthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthEast7"), 0.06));

            var runEast = new Animation(AnimationType.Looped);
            runEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkEast0"), 0.06));
            runEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkEast1"), 0.06));
            runEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkEast2"), 0.06));
            runEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkEast3"), 0.06));
            runEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkEast4"), 0.06));
            runEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkEast5"), 0.06));
            runEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkEast6"), 0.06));
            runEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkEast7"), 0.06));

            var runSouthEast = new Animation(AnimationType.Looped);
            runSouthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthEast0"), 0.06));
            runSouthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthEast1"), 0.06));
            runSouthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthEast2"), 0.06));
            runSouthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthEast3"), 0.06));
            runSouthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthEast4"), 0.06));
            runSouthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthEast5"), 0.06));
            runSouthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthEast6"), 0.06));
            runSouthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthEast7"), 0.06));

            var runSouth = new Animation(AnimationType.Looped);
            runSouth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouth0"), 0.06));
            runSouth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouth1"), 0.06));
            runSouth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouth2"), 0.06));
            runSouth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouth3"), 0.06));
            runSouth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouth4"), 0.06));
            runSouth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouth5"), 0.06));
            runSouth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouth6"), 0.06));
            runSouth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouth7"), 0.06));

            var runSouthWest = new Animation(AnimationType.Looped);
            runSouthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthWest0"), 0.06));
            runSouthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthWest1"), 0.06));
            runSouthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthWest2"), 0.06));
            runSouthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthWest3"), 0.06));
            runSouthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthWest4"), 0.06));
            runSouthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthWest5"), 0.06));
            runSouthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthWest6"), 0.06));
            runSouthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkSouthWest7"), 0.06));

            var runWest = new Animation(AnimationType.Looped);
            runWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkWest0"), 0.06));
            runWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkWest1"), 0.06));
            runWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkWest2"), 0.06));
            runWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkWest3"), 0.06));
            runWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkWest4"), 0.06));
            runWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkWest5"), 0.06));
            runWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkWest6"), 0.06));
            runWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkWest7"), 0.06));

            var runNorthWest = new Animation(AnimationType.Looped);
            runNorthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthWest0"), 0.06));
            runNorthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthWest1"), 0.06));
            runNorthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthWest2"), 0.06));
            runNorthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthWest3"), 0.06));
            runNorthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthWest4"), 0.06));
            runNorthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthWest5"), 0.06));
            runNorthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthWest6"), 0.06));
            runNorthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("walkNorthWest7"), 0.06));

            humanAnimationList.Add("idleSouthEast", idleSouthEast);
            humanAnimationList.Add("idleSouth", idleSouth);
            humanAnimationList.Add("idleSouthWest", idleSouthWest);
            humanAnimationList.Add("idleWest", idleWest);
            humanAnimationList.Add("idleNorthWest", idleNorthWest);
            humanAnimationList.Add("idleNorth", idleNorth);
            humanAnimationList.Add("idleNorthEast", idleNorthEast);
            humanAnimationList.Add("idleEast", idleEast);

            humanAnimationList.Add("walkNorth", walkNorth);
            humanAnimationList.Add("walkNorthEast", walkNorthEast);
            humanAnimationList.Add("walkEast", walkEast);
            humanAnimationList.Add("walkSouthEast", walkSouthEast);
            humanAnimationList.Add("walkSouth", walkSouth);
            humanAnimationList.Add("walkSouthWest", walkSouthWest);
            humanAnimationList.Add("walkWest", walkWest);
            humanAnimationList.Add("walkNorthWest", walkNorthWest);

            humanAnimationList.Add("runNorth", runNorth);
            humanAnimationList.Add("runNorthEast", runNorthEast);
            humanAnimationList.Add("runEast", runEast);
            humanAnimationList.Add("runSouthEast", runSouthEast);
            humanAnimationList.Add("runSouth", runSouth);
            humanAnimationList.Add("runSouthWest", runSouthWest);
            humanAnimationList.Add("runWest", runWest);
            humanAnimationList.Add("runNorthWest", runNorthWest);

            var humanSprite = new AnimatedSprite("human", humanSpriteSheet, new Vector2(16, 32), new BoundingBox(new Vector3(0.2f, 0.2f, 0.2f), new Vector3(0.8f, 0.8f, 0.8f)), humanAnimationList);

            return humanSprite;
        }

        private AnimatedSprite BuildZombieSprite()
        {
            var zombieSpriteSheet = SpriteSheetLoader.FromPath("Content/SpriteSheets/zombie");

            var zombieAnimationList = new AnimationList();
            var idleSouthEast = new Animation(AnimationType.RunOnce);
            idleSouthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("idleSouthEast"), 1.0));
            var idleSouth = new Animation(AnimationType.RunOnce);
            idleSouth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("idleSouth"), 1.0));
            var idleSouthWest = new Animation(AnimationType.RunOnce);
            idleSouthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("idleSouthWest"), 1.0));
            var idleWest = new Animation(AnimationType.RunOnce);
            idleWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("idleWest"), 1.0));
            var idleNorthWest = new Animation(AnimationType.RunOnce);
            idleNorthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("idleNorthWest"), 1.0));
            var idleNorth = new Animation(AnimationType.RunOnce);
            idleNorth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("idleNorth"), 1.0));
            var idleNorthEast = new Animation(AnimationType.RunOnce);
            idleNorthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("idleNorthEast"), 1.0));
            var idleEast = new Animation(AnimationType.RunOnce);
            idleEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("idleEast"), 1.0));

            zombieAnimationList.Add("idleSouthEast", idleSouthEast);
            zombieAnimationList.Add("idleSouth", idleSouth);
            zombieAnimationList.Add("idleSouthWest", idleSouthWest);
            zombieAnimationList.Add("idleWest", idleWest);
            zombieAnimationList.Add("idleNorthWest", idleNorthWest);
            zombieAnimationList.Add("idleNorth", idleNorth);
            zombieAnimationList.Add("idleNorthEast", idleNorthEast);
            zombieAnimationList.Add("idleEast", idleEast);

            var zombieSprite = new AnimatedSprite("zombie", zombieSpriteSheet, new Vector2(16, 40), new BoundingBox(new Vector3(0.2f, 0.2f, 0.2f), new Vector3(0.8f, 0.8f, 0.8f)), zombieAnimationList);

            return zombieSprite;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            FrameRater.NewUpdate((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            GameState.GameTime = gameTime;
            Engine.Input.Mouse.Update(gameTime);

            _camera.Update(gameTime);
            _map.Update(gameTime);

            _virtualScreen.Update();

            base.Update(gameTime);

            FrameRater.EndUpdate((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            FrameRater.NewFrame((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds);

            _virtualScreen.BeginCapture();
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            _drawingManager.Draw(_spriteBatch);
            //_spriteBatch.End();

            //_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            _uiManager.Draw(_spriteBatch);
            _spriteBatch.End();

            _virtualScreen.EndCapture();

            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            _virtualScreen.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);

            FrameRater.EndFrame((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds);
        }
    }
}
