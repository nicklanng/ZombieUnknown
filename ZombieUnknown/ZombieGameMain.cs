using System;
using System.Collections.Generic;
using System.Linq;
using Engine;
using Engine.AI.Steering;
using Engine.Drawing;
using Engine.Entities;
using Engine.Maps;
using Engine.Serialization;
using Engine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZombieUnknown.AI.BehaviorTrees;
using ZombieUnknown.Entities;
using ZombieUnknown.Entities.Mobiles;
using Console = Engine.Drawing.Console;
using Mouse = Engine.Input.Mouse;

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
            Content.RootDirectory = "Content";

#if WINDOWS
            _graphics = new GraphicsDeviceManager(this)
                {

                    PreferredBackBufferWidth = 1280,
                    PreferredBackBufferHeight = 720,
                    IsFullScreen = false
                    //PreferredBackBufferWidth = 2560,
                    //PreferredBackBufferHeight = 1440,
                    //IsFullScreen = true
                };

            Window.IsBorderless = true;
#endif
#if MAC
            _graphics = new GraphicsDeviceManager(this)
            {

                PreferredBackBufferWidth = 1366,
                PreferredBackBufferHeight = 768,
                IsFullScreen = true
            };

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
            GameState.GraphicsDevice = GraphicsDevice;
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _virtualScreen = new VirtualScreen(640, 360);
            GameState.VirtualScreen = _virtualScreen;

#if WINDOWS
            Window.ClientSizeChanged += Window_ClientSizeChanged;
            Window.AllowUserResizing = true;
#endif

            _camera = new Camera(new Vector2(_virtualScreen.VirtualWidth, _virtualScreen.VirtualHeight), 100, new IsometricConfiguration());
            _camera.ScreenCenterPosition = new Vector2(3, 143);

            _drawingManager = new DrawingManager(_camera);
            _uiManager = new UIManager();

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
            GameState.Actors = new List<IActor>();

            // All of the initialization stuff should be somewhere else, and probably load from data files
            var fontSpriteSheet = SpriteSheetLoader.FromPath("Content/Fonts/dbmf_4x5_box");
            var terrainSpriteSheet = SpriteSheetLoader.FromPath("Content/SpriteSheets/xcom-forest");
            var wallSpriteSheet = SpriteSheetLoader.FromPath("Content/SpriteSheets/walls");
            var itemsSpriteSheet = SpriteSheetLoader.FromPath("Content/SpriteSheets/items");
            var agricultureSpriteSheet = SpriteSheetLoader.FromPath("Content/SpriteSheets/agriculture");
            var humanSpriteSheet = SpriteSheetLoader.FromPath("Content/SpriteSheets/civf");
            var zombieSpriteSheet = SpriteSheetLoader.FromPath("Content/SpriteSheets/civm");
            var uiSpriteSheet = SpriteSheetLoader.FromPath("Content/SpriteSheets/ui");

            var cursorSprite = new StaticSprite("cursor", uiSpriteSheet, Vector2.Zero, "cursor");
            ResourceManager.RegisterSprite(cursorSprite);

            var terrainSprites = new List<Sprite>
            {
                new StaticSprite("grass", terrainSpriteSheet, new Vector2(16, 32), "grass"),
                new StaticSprite("grass", terrainSpriteSheet, new Vector2(16, 32), "tallGrass1"),
                new StaticSprite("grass", terrainSpriteSheet, new Vector2(16, 32), "tallGrass2")
            };
            var zombieSprite = BuildZombieSprite(zombieSpriteSheet);
            ResourceManager.RegisterSprite(zombieSprite);

            var font = new Font(fontSpriteSheet);

            var leftWallSprite = new StaticSprite("left", wallSpriteSheet, new Vector2(16, 40), "left");
            ResourceManager.RegisterSprite(leftWallSprite);
            var rightWallSprite = new StaticSprite("right", wallSpriteSheet, new Vector2(16, 40), "right");
            ResourceManager.RegisterSprite(rightWallSprite);
            var internalJoinWallSprite = new StaticSprite("internalJoin", wallSpriteSheet, new Vector2(16, 40), "join");
            ResourceManager.RegisterSprite(internalJoinWallSprite);
            var externalJoinWallSprite = new StaticSprite("externalJoin", wallSpriteSheet, new Vector2(16, 41), "join");
            ResourceManager.RegisterSprite(externalJoinWallSprite);

            var humanSprite = BuildHumanSprite(humanSpriteSheet);
            ResourceManager.RegisterSprite(humanSprite);

            var deadHumansprite = new StaticSprite("deadHuman", humanSpriteSheet, new Vector2(16, 32), "dead");
            ResourceManager.RegisterSprite(deadHumansprite);

            var foodSprite = new StaticSprite("food", itemsSpriteSheet, new Vector2(16, 40), "food");
            ResourceManager.RegisterSprite(foodSprite);
            var lampSprite = new StaticSprite("lamp", itemsSpriteSheet, new Vector2(16, 40), "lamp");
            ResourceManager.RegisterSprite(lampSprite);

            var cultivatedLandSprite = new StaticSprite("cultivatedLand", agricultureSpriteSheet, new Vector2(16, 40), "cultivatedLand");
            ResourceManager.RegisterSprite(cultivatedLandSprite);

            var wheatAnimationList = new AnimationList();
            var sown = new Animation(AnimationType.RunOnce);
            sown.AddFrame(new AnimationFrame(agricultureSpriteSheet.GetFrameRectangle("wheatSown"), 1.0));
            var growing = new Animation(AnimationType.RunOnce);
            growing.AddFrame(new AnimationFrame(agricultureSpriteSheet.GetFrameRectangle("wheatGrowing"), 1.0));
            var grown = new Animation(AnimationType.RunOnce);
            grown.AddFrame(new AnimationFrame(agricultureSpriteSheet.GetFrameRectangle("wheatGrown"), 1.0));
            wheatAnimationList.Add("sown", sown);
            wheatAnimationList.Add("growing", growing);
            wheatAnimationList.Add("grown", grown);
            var wheatSprite = new AnimatedSprite("wheat", agricultureSpriteSheet, new Vector2(16, 40), wheatAnimationList);
            ResourceManager.RegisterSprite(wheatSprite);
            
            var tiles = new Tile[(int)_mapSize.X, (int)_mapSize.Y];
            for (var y = 0; y < (int) _mapSize.Y; y++)
            {
                for (var x = 0; x < (int)_mapSize.X; x++)
                {
                    tiles[x, y] = new Tile(new Vector2(x, y), terrainSprites[0], null, null, null);
                }
            }
            var rand = new Random();
            
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

            tiles[10, 10].SetLeftWall(leftWallSprite);
            tiles[11, 10].SetLeftWall(leftWallSprite);
            tiles[12, 10].SetLeftWall(leftWallSprite);


            _map = new Map((short)_mapSize.X, (short)_mapSize.Y, tiles);
            GameState.Map = _map;

            _pathfindingMap = new PathfindingMap();
            GameState.PathfindingMap = _pathfindingMap;
            
            var light = new PhantomLight("light", new Coordinate(2, 1), Color.Blue, 10);
            GameController.SpawnEntity(light);

            var light2 = new PhantomLight("light2", new Coordinate(4, 5), Color.White, 10);
            GameController.SpawnEntity(light2);

            var listOfTiles = new List<Coordinate>();
            for (var y = 0; y < (int)_mapSize.Y - 1; y++)
            {
                for (var x = 0; x < (int)_mapSize.X - 1; x++)
                {
                    listOfTiles.Add(new Coordinate(x, y));
                }
            }
            listOfTiles = listOfTiles.OrderBy(x => Guid.NewGuid()).ToList();

            for (var i = 0; i < 50; i++)
            {
                var human = new Human("human" + i, (Vector2)listOfTiles.ElementAt(0) + new Vector2(0.5f, 0.5f));
                listOfTiles.RemoveAt(0);
                human.ForceFaceDirection(Direction.North);
                GameController.SpawnEntity(human);
                GameState.ZombieTarget = human;
            }

            for (var i = 0; i < 0; i++)
            {
                var zombie = new Zombie("zombie" + i, (Vector2)listOfTiles.ElementAt(0) + new Vector2(0.5f, 0.5f));
                listOfTiles.RemoveAt(0);
                zombie.ForceFaceDirection(Direction.North);
                GameController.SpawnEntity(zombie);
            }

            var food = new FoodContainer("food", new Coordinate(4, 5));
            GameController.SpawnEntity(food);
            _pathfindingMap.AddBlockage(food);

            var lamp = new Lamp("lamp", new Coordinate(13, 14));
            GameController.SpawnEntity(lamp);
            _pathfindingMap.AddBlockage(lamp);

            var lamp2 = new Lamp("lamp2", new Coordinate(13, 4));
            _map.AddEntity(lamp2);
            _pathfindingMap.AddBlockage(lamp2);

            var cultivatedLand = new CultivatedLand("cultivatedLand", new Coordinate(10, 3));
            GameController.SpawnEntity(cultivatedLand);

            for (var x = 10; x < 15; x++)
            {
                for (var y = 5; y < 9; y++)
                {
                    var h = new Wheat("wheat", new Coordinate(x, y));
                    GameController.SpawnEntity(h);
                }
            }

            _lightMap = new LightMap(_map, new Color(0.15f, 0.15f, 0.25f));
            //_lightMap = new LightMap(_map, new Color(0.8f, 0.8f, 0.8f));

            Mouse.Initialize();
            Console.Initialize(_spriteBatch, font, 1);
            Console.WriteLine("Post-apocalyptic Management Game");
            FrameRater.Initialize(_spriteBatch, font);

            _drawingManager.RegisterProvider(_map);
            //_drawingManager.RegisterProvider(_pathfindingMap);
            _uiManager.RegisterProvider(Console.DrawingProvider);
            _uiManager.RegisterProvider(FrameRater.DrawingProvider);
            _uiManager.RegisterProvider(Mouse.DrawingProvider);

            GameState.InteractionObject = food;
            //GameState.MainCharacter = human;
        }

        private AnimatedSprite BuildHumanSprite(SpriteSheet humanSpriteSheet)
        {
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


            var dying = new Animation(AnimationType.RunOnce);
            dying.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("dying0"), 0.2));
            dying.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("dying1"), 0.2));
            dying.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("dying2"), 0.2));
            dying.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("dead"), 0.2));

            var interactSouthEast = new Animation(AnimationType.RunOnce);
            interactSouthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("interactSouthEast"), 1.0));
            var interactSouth = new Animation(AnimationType.RunOnce);
            interactSouth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("interactSouth"), 1.0));
            var interactSouthWest = new Animation(AnimationType.RunOnce);
            interactSouthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("interactSouthWest"), 1.0));
            var interactWest = new Animation(AnimationType.RunOnce);
            interactWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("interactWest"), 1.0));
            var interactNorthWest = new Animation(AnimationType.RunOnce);
            interactNorthWest.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("interactNorthWest"), 1.0));
            var interactNorth = new Animation(AnimationType.RunOnce);
            interactNorth.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("interactNorth"), 1.0));
            var interactNorthEast = new Animation(AnimationType.RunOnce);
            interactNorthEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("interactNorthEast"), 1.0));
            var interactEast = new Animation(AnimationType.RunOnce);
            interactEast.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("interactEast"), 1.0));

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

            humanAnimationList.Add("dyingNorth", dying);
            humanAnimationList.Add("dyingNorthEast", dying);
            humanAnimationList.Add("dyingEast", dying);
            humanAnimationList.Add("dyingSouthEast", dying);
            humanAnimationList.Add("dyingSouth", dying);
            humanAnimationList.Add("dyingSouthWest", dying);
            humanAnimationList.Add("dyingWest", dying);
            humanAnimationList.Add("dyingNorthWest", dying);

            humanAnimationList.Add("interactSouthEast", interactSouthEast);
            humanAnimationList.Add("interactSouth", interactSouth);
            humanAnimationList.Add("interactSouthWest", interactSouthWest);
            humanAnimationList.Add("interactWest", interactWest);
            humanAnimationList.Add("interactNorthWest", interactNorthWest);
            humanAnimationList.Add("interactNorth", interactNorth);
            humanAnimationList.Add("interactNorthEast", interactNorthEast);
            humanAnimationList.Add("interactEast", interactEast);

            var humanSprite = new AnimatedSprite("human", humanSpriteSheet, new Vector2(16, 40), humanAnimationList);

            return humanSprite;
        }
        
        private AnimatedSprite BuildZombieSprite(SpriteSheet zombieSpriteSheet)
        {
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

            var walkNorth = new Animation(AnimationType.Looped);
            walkNorth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorth0"), 0.45));
            walkNorth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorth1"), 0.45));
            walkNorth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorth2"), 0.45));
            walkNorth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorth3"), 0.45));
            walkNorth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorth4"), 0.45));
            walkNorth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorth5"), 0.45));
            walkNorth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorth6"), 0.45));
            walkNorth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorth7"), 0.45));

            var walkNorthEast = new Animation(AnimationType.Looped);
            walkNorthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthEast0"), 0.45));
            walkNorthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthEast1"), 0.45));
            walkNorthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthEast2"), 0.45));
            walkNorthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthEast3"), 0.45));
            walkNorthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthEast4"), 0.45));
            walkNorthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthEast5"), 0.45));
            walkNorthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthEast6"), 0.45));
            walkNorthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthEast7"), 0.45));

            var walkEast = new Animation(AnimationType.Looped);
            walkEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkEast0"), 0.45));
            walkEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkEast1"), 0.45));
            walkEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkEast2"), 0.45));
            walkEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkEast3"), 0.45));
            walkEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkEast4"), 0.45));
            walkEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkEast5"), 0.45));
            walkEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkEast6"), 0.45));
            walkEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkEast7"), 0.45));

            var walkSouthEast = new Animation(AnimationType.Looped);
            walkSouthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthEast0"), 0.45));
            walkSouthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthEast1"), 0.45));
            walkSouthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthEast2"), 0.45));
            walkSouthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthEast3"), 0.45));
            walkSouthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthEast4"), 0.45));
            walkSouthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthEast5"), 0.45));
            walkSouthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthEast6"), 0.45));
            walkSouthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthEast7"), 0.45));

            var walkSouth = new Animation(AnimationType.Looped);
            walkSouth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouth0"), 0.45));
            walkSouth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouth1"), 0.45));
            walkSouth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouth2"), 0.45));
            walkSouth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouth3"), 0.45));
            walkSouth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouth4"), 0.45));
            walkSouth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouth5"), 0.45));
            walkSouth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouth6"), 0.45));
            walkSouth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouth7"), 0.45));

            var walkSouthWest = new Animation(AnimationType.Looped);
            walkSouthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthWest0"), 0.45));
            walkSouthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthWest1"), 0.45));
            walkSouthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthWest2"), 0.45));
            walkSouthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthWest3"), 0.45));
            walkSouthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthWest4"), 0.45));
            walkSouthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthWest5"), 0.45));
            walkSouthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthWest6"), 0.45));
            walkSouthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthWest7"), 0.45));

            var walkWest = new Animation(AnimationType.Looped);
            walkWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkWest0"), 0.45));
            walkWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkWest1"), 0.45));
            walkWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkWest2"), 0.45));
            walkWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkWest3"), 0.45));
            walkWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkWest4"), 0.45));
            walkWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkWest5"), 0.45));
            walkWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkWest6"), 0.45));
            walkWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkWest7"), 0.45));

            var walkNorthWest = new Animation(AnimationType.Looped);
            walkNorthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthWest0"), 0.45));
            walkNorthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthWest1"), 0.45));
            walkNorthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthWest2"), 0.45));
            walkNorthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthWest3"), 0.45));
            walkNorthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthWest4"), 0.45));
            walkNorthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthWest5"), 0.45));
            walkNorthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthWest6"), 0.45));
            walkNorthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthWest7"), 0.45));

            var runNorth = new Animation(AnimationType.Looped);
            runNorth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorth0"), 0.06));
            runNorth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorth1"), 0.06));
            runNorth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorth2"), 0.06));
            runNorth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorth3"), 0.06));
            runNorth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorth4"), 0.06));
            runNorth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorth5"), 0.06));
            runNorth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorth6"), 0.06));
            runNorth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorth7"), 0.06));

            var runNorthEast = new Animation(AnimationType.Looped);
            runNorthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthEast0"), 0.06));
            runNorthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthEast1"), 0.06));
            runNorthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthEast2"), 0.06));
            runNorthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthEast3"), 0.06));
            runNorthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthEast4"), 0.06));
            runNorthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthEast5"), 0.06));
            runNorthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthEast6"), 0.06));
            runNorthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthEast7"), 0.06));

            var runEast = new Animation(AnimationType.Looped);
            runEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkEast0"), 0.06));
            runEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkEast1"), 0.06));
            runEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkEast2"), 0.06));
            runEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkEast3"), 0.06));
            runEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkEast4"), 0.06));
            runEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkEast5"), 0.06));
            runEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkEast6"), 0.06));
            runEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkEast7"), 0.06));

            var runSouthEast = new Animation(AnimationType.Looped);
            runSouthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthEast0"), 0.06));
            runSouthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthEast1"), 0.06));
            runSouthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthEast2"), 0.06));
            runSouthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthEast3"), 0.06));
            runSouthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthEast4"), 0.06));
            runSouthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthEast5"), 0.06));
            runSouthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthEast6"), 0.06));
            runSouthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthEast7"), 0.06));

            var runSouth = new Animation(AnimationType.Looped);
            runSouth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouth0"), 0.06));
            runSouth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouth1"), 0.06));
            runSouth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouth2"), 0.06));
            runSouth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouth3"), 0.06));
            runSouth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouth4"), 0.06));
            runSouth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouth5"), 0.06));
            runSouth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouth6"), 0.06));
            runSouth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouth7"), 0.06));

            var runSouthWest = new Animation(AnimationType.Looped);
            runSouthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthWest0"), 0.06));
            runSouthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthWest1"), 0.06));
            runSouthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthWest2"), 0.06));
            runSouthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthWest3"), 0.06));
            runSouthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthWest4"), 0.06));
            runSouthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthWest5"), 0.06));
            runSouthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthWest6"), 0.06));
            runSouthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkSouthWest7"), 0.06));

            var runWest = new Animation(AnimationType.Looped);
            runWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkWest0"), 0.06));
            runWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkWest1"), 0.06));
            runWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkWest2"), 0.06));
            runWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkWest3"), 0.06));
            runWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkWest4"), 0.06));
            runWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkWest5"), 0.06));
            runWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkWest6"), 0.06));
            runWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkWest7"), 0.06));

            var runNorthWest = new Animation(AnimationType.Looped);
            runNorthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthWest0"), 0.06));
            runNorthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthWest1"), 0.06));
            runNorthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthWest2"), 0.06));
            runNorthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthWest3"), 0.06));
            runNorthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthWest4"), 0.06));
            runNorthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthWest5"), 0.06));
            runNorthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthWest6"), 0.06));
            runNorthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("walkNorthWest7"), 0.06));


            var dying = new Animation(AnimationType.RunOnce);
            dying.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("dying0"), 0.2));
            dying.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("dying1"), 0.2));
            dying.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("dying2"), 0.2));
            dying.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("dead"), 0.2));

            var interactSouthEast = new Animation(AnimationType.RunOnce);
            interactSouthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("interactSouthEast"), 1.0));
            var interactSouth = new Animation(AnimationType.RunOnce);
            interactSouth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("interactSouth"), 1.0));
            var interactSouthWest = new Animation(AnimationType.RunOnce);
            interactSouthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("interactSouthWest"), 1.0));
            var interactWest = new Animation(AnimationType.RunOnce);
            interactWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("interactWest"), 1.0));
            var interactNorthWest = new Animation(AnimationType.RunOnce);
            interactNorthWest.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("interactNorthWest"), 1.0));
            var interactNorth = new Animation(AnimationType.RunOnce);
            interactNorth.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("interactNorth"), 1.0));
            var interactNorthEast = new Animation(AnimationType.RunOnce);
            interactNorthEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("interactNorthEast"), 1.0));
            var interactEast = new Animation(AnimationType.RunOnce);
            interactEast.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("interactEast"), 1.0));

            zombieAnimationList.Add("idleSouthEast", idleSouthEast);
            zombieAnimationList.Add("idleSouth", idleSouth);
            zombieAnimationList.Add("idleSouthWest", idleSouthWest);
            zombieAnimationList.Add("idleWest", idleWest);
            zombieAnimationList.Add("idleNorthWest", idleNorthWest);
            zombieAnimationList.Add("idleNorth", idleNorth);
            zombieAnimationList.Add("idleNorthEast", idleNorthEast);
            zombieAnimationList.Add("idleEast", idleEast);

            zombieAnimationList.Add("walkNorth", walkNorth);
            zombieAnimationList.Add("walkNorthEast", walkNorthEast);
            zombieAnimationList.Add("walkEast", walkEast);
            zombieAnimationList.Add("walkSouthEast", walkSouthEast);
            zombieAnimationList.Add("walkSouth", walkSouth);
            zombieAnimationList.Add("walkSouthWest", walkSouthWest);
            zombieAnimationList.Add("walkWest", walkWest);
            zombieAnimationList.Add("walkNorthWest", walkNorthWest);

            zombieAnimationList.Add("runNorth", runNorth);
            zombieAnimationList.Add("runNorthEast", runNorthEast);
            zombieAnimationList.Add("runEast", runEast);
            zombieAnimationList.Add("runSouthEast", runSouthEast);
            zombieAnimationList.Add("runSouth", runSouth);
            zombieAnimationList.Add("runSouthWest", runSouthWest);
            zombieAnimationList.Add("runWest", runWest);
            zombieAnimationList.Add("runNorthWest", runNorthWest);

            zombieAnimationList.Add("dyingNorth", dying);
            zombieAnimationList.Add("dyingNorthEast", dying);
            zombieAnimationList.Add("dyingEast", dying);
            zombieAnimationList.Add("dyingSouthEast", dying);
            zombieAnimationList.Add("dyingSouth", dying);
            zombieAnimationList.Add("dyingSouthWest", dying);
            zombieAnimationList.Add("dyingWest", dying);
            zombieAnimationList.Add("dyingNorthWest", dying);

            zombieAnimationList.Add("interactSouthEast", interactSouthEast);
            zombieAnimationList.Add("interactSouth", interactSouth);
            zombieAnimationList.Add("interactSouthWest", interactSouthWest);
            zombieAnimationList.Add("interactWest", interactWest);
            zombieAnimationList.Add("interactNorthWest", interactNorthWest);
            zombieAnimationList.Add("interactNorth", interactNorth);
            zombieAnimationList.Add("interactNorthEast", interactNorthEast);
            zombieAnimationList.Add("interactEast", interactEast);

            var zombieSprite = new AnimatedSprite("zombie", zombieSpriteSheet, new Vector2(16, 40), zombieAnimationList);

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
            Engine.Input.Mouse.Instance.Update(gameTime);

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
