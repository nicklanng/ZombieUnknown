using System;
using System.Collections.Generic;
using System.Threading;
using Engine;
using Engine.Drawing;
using Engine.Entities;
using Engine.Maps;
using Engine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
                    
                    PreferredBackBufferWidth = 1366,
                    PreferredBackBufferHeight = 768,
                    IsFullScreen = true
                };
            Content.RootDirectory = "Content";

#if WINDOWS
            Window.IsBorderless = true;
#endif

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
            // All of the initialization stuff should be somewhere else, and probably load from data files

            var fontTexture = Texture2D.FromStream(GraphicsDevice, TitleContainer.OpenStream("Content/Fonts/dbmf_4x5_box.png"));
            var fontSpriteSheet = new SpriteSheet("font", fontTexture);
            fontSpriteSheet.AddFrame("a", new Rectangle(0, 0, 4, 5));
            fontSpriteSheet.AddFrame("b", new Rectangle(4, 0, 4, 5));
            fontSpriteSheet.AddFrame("c", new Rectangle(8, 0, 4, 5));
            fontSpriteSheet.AddFrame("d", new Rectangle(12, 0, 4, 5));
            fontSpriteSheet.AddFrame("e", new Rectangle(16, 0, 4, 5));
            fontSpriteSheet.AddFrame("f", new Rectangle(20, 0, 4, 5));
            fontSpriteSheet.AddFrame("g", new Rectangle(24, 0, 4, 5));
            fontSpriteSheet.AddFrame("h", new Rectangle(28, 0, 4, 5));
            fontSpriteSheet.AddFrame("i", new Rectangle(32, 0, 4, 5));
            fontSpriteSheet.AddFrame("j", new Rectangle(36, 0, 4, 5));
            fontSpriteSheet.AddFrame("k", new Rectangle(40, 0, 4, 5));
            fontSpriteSheet.AddFrame("l", new Rectangle(44, 0, 4, 5));
            fontSpriteSheet.AddFrame("m", new Rectangle(48, 0, 4, 5));
            fontSpriteSheet.AddFrame("n", new Rectangle(52, 0, 4, 5));
            fontSpriteSheet.AddFrame("o", new Rectangle(56, 0, 4, 5));
            fontSpriteSheet.AddFrame("p", new Rectangle(60, 0, 4, 5));
            fontSpriteSheet.AddFrame("q", new Rectangle(64, 0, 4, 5));
            fontSpriteSheet.AddFrame("r", new Rectangle(68, 0, 4, 5));
            fontSpriteSheet.AddFrame("s", new Rectangle(72, 0, 4, 5));
            fontSpriteSheet.AddFrame("t", new Rectangle(76, 0, 4, 5));
            fontSpriteSheet.AddFrame("u", new Rectangle(80, 0, 4, 5));
            fontSpriteSheet.AddFrame("v", new Rectangle(84, 0, 4, 5));
            fontSpriteSheet.AddFrame("w", new Rectangle(88, 0, 4, 5));
            fontSpriteSheet.AddFrame("x", new Rectangle(92, 0, 4, 5));
            fontSpriteSheet.AddFrame("y", new Rectangle(96, 0, 4, 5));
            fontSpriteSheet.AddFrame("z", new Rectangle(100, 0, 4, 5));
            fontSpriteSheet.AddFrame("0", new Rectangle(104, 0, 4, 5));
            fontSpriteSheet.AddFrame("1", new Rectangle(108, 0, 4, 5));
            fontSpriteSheet.AddFrame("2", new Rectangle(112, 0, 4, 5));
            fontSpriteSheet.AddFrame("3", new Rectangle(116, 0, 4, 5));
            fontSpriteSheet.AddFrame("4", new Rectangle(120, 0, 4, 5));
            fontSpriteSheet.AddFrame("5", new Rectangle(124, 0, 4, 5));
            fontSpriteSheet.AddFrame("6", new Rectangle(128, 0, 4, 5));
            fontSpriteSheet.AddFrame("7", new Rectangle(132, 0, 4, 5));
            fontSpriteSheet.AddFrame("8", new Rectangle(136, 0, 4, 5));
            fontSpriteSheet.AddFrame("9", new Rectangle(140, 0, 4, 5));
            fontSpriteSheet.AddFrame(" ", new Rectangle(12, 5, 4, 5));
            fontSpriteSheet.AddFrame(",", new Rectangle(0, 10, 4, 5));
            fontSpriteSheet.AddFrame(".", new Rectangle(4, 10, 4, 5));
            fontSpriteSheet.AddFrame(":", new Rectangle(8, 10, 4, 5));
            fontSpriteSheet.AddFrame(";", new Rectangle(12, 10, 4, 5));
            fontSpriteSheet.AddFrame("!", new Rectangle(16, 10, 4, 5));
            fontSpriteSheet.AddFrame("?", new Rectangle(20, 10, 4, 5));
            fontSpriteSheet.AddFrame("_", new Rectangle(24, 10, 4, 5));
            fontSpriteSheet.AddFrame("-", new Rectangle(28, 10, 4, 5));
            fontSpriteSheet.AddFrame("+", new Rectangle(32, 10, 4, 5));
            fontSpriteSheet.AddFrame("=", new Rectangle(36, 10, 4, 5));
            fontSpriteSheet.AddFrame("*", new Rectangle(40, 10, 4, 5));
            fontSpriteSheet.AddFrame("'", new Rectangle(44, 10, 4, 5));
            fontSpriteSheet.AddFrame("\"", new Rectangle(48, 10, 4, 5));
            fontSpriteSheet.AddFrame("[", new Rectangle(52, 10, 4, 5));
            fontSpriteSheet.AddFrame("]", new Rectangle(56, 10, 4, 5));
            fontSpriteSheet.AddFrame("(", new Rectangle(60, 10, 4, 5));
            fontSpriteSheet.AddFrame(")", new Rectangle(64, 10, 4, 5));
            fontSpriteSheet.AddFrame("<", new Rectangle(68, 10, 4, 5));
            fontSpriteSheet.AddFrame(">", new Rectangle(72, 10, 4, 5));
            fontSpriteSheet.AddFrame("/", new Rectangle(76, 10, 4, 5));
            fontSpriteSheet.AddFrame("\\", new Rectangle(80, 10, 4, 5));
            var font = new Font(fontSpriteSheet);

            var terrainTexture = Texture2D.FromStream(GraphicsDevice, TitleContainer.OpenStream("Content/SpriteSheets/xcom-forest.png"));
            //var cursorTexture = Texture2D.FromStream(GraphicsDevice, TitleContainer.OpenStream("Content/SpriteSheets/cursor.png"));
            var wallsTexture = Texture2D.FromStream(GraphicsDevice, TitleContainer.OpenStream("Content/SpriteSheets/walls.png"));
            var debugIconTexture = Texture2D.FromStream(GraphicsDevice, TitleContainer.OpenStream("Content/SpriteSheets/debugIcons.png"));
            var zombieTexture = Texture2D.FromStream(GraphicsDevice, TitleContainer.OpenStream("Content/SpriteSheets/zombie.png"));

            var terrainSpriteSheet = new SpriteSheet("terrain", terrainTexture);
            terrainSpriteSheet.AddFrame("grass", new Rectangle(0, 0, 32, 40));
            terrainSpriteSheet.AddFrame("leaves1", new Rectangle(32, 0, 32, 40));
            terrainSpriteSheet.AddFrame("leaves2", new Rectangle(64, 0, 32, 40));
            terrainSpriteSheet.AddFrame("water", new Rectangle(96, 0, 32, 40));
            terrainSpriteSheet.AddFrame("tallGrass1", new Rectangle(128, 0, 32, 40));
            terrainSpriteSheet.AddFrame("tallGrass2", new Rectangle(160, 0, 32, 40));
            terrainSpriteSheet.AddFrame("swamp1", new Rectangle(192, 0, 32, 40));
            terrainSpriteSheet.AddFrame("swamp2", new Rectangle(224, 0, 32, 40));
            terrainSpriteSheet.AddFrame("deadTree1", new Rectangle(0, 40, 32, 40));
            terrainSpriteSheet.AddFrame("deadTree2", new Rectangle(32, 40, 32, 40));
            terrainSpriteSheet.AddFrame("deadTree3", new Rectangle(64, 40, 32, 40));
            terrainSpriteSheet.AddFrame("deadTree4", new Rectangle(96, 40, 32, 40));

            var wallSpriteSheet = new SpriteSheet("walls", wallsTexture);
            wallSpriteSheet.AddFrame("left", new Rectangle(0, 0, 32, 48));
            wallSpriteSheet.AddFrame("right", new Rectangle(32, 0, 32, 48));
            wallSpriteSheet.AddFrame("join", new Rectangle(64, 0, 32, 48));

            var debugIconsSpriteSheet = new SpriteSheet("debugIcons", debugIconTexture);
            debugIconsSpriteSheet.AddFrame("light", new Rectangle(0, 0, 32, 48));

            var zombieSpriteSheet = new SpriteSheet("zombie", zombieTexture);
            zombieSpriteSheet.AddFrame("standingDown", new Rectangle(0, 0, 32, 48));
            zombieSpriteSheet.AddFrame("standingDownLeft", new Rectangle(32, 0, 32, 48));
            zombieSpriteSheet.AddFrame("standingLeft", new Rectangle(64, 0, 32, 48));
            zombieSpriteSheet.AddFrame("standingUpLeft", new Rectangle(96, 0, 32, 48));
            zombieSpriteSheet.AddFrame("standingUp", new Rectangle(128, 0, 32, 48));
            zombieSpriteSheet.AddFrame("standingUpRight", new Rectangle(160, 0, 32, 48));
            zombieSpriteSheet.AddFrame("standingRight", new Rectangle(192, 0, 32, 48));
            zombieSpriteSheet.AddFrame("standingDownRight", new Rectangle(224, 0, 32, 48));

            var leftWallSprite = new StaticSprite("left", wallSpriteSheet, new Vector2(16, 40), new BoundingBox(new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0.9f, 0.1f, 0.9f)), "left");
            var rightWallSprite = new StaticSprite("right", wallSpriteSheet, new Vector2(16, 40), new BoundingBox(new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0.1f, 0.9f, 0.9f)), "right");
            var joinWallSprite = new StaticSprite("join", wallSpriteSheet, new Vector2(16, 40), new BoundingBox(new Vector3(0.1f, 0.1f, 0.1f), new Vector3(0.2f, 0.2f, 0.9f)), "join");

            var humanSprite = BuildHumanSprite();
            var zombieSprite = BuildZombieSprite();

            var terrainSprites = new List<Sprite>
                {
                    new StaticSprite("grass", terrainSpriteSheet, new Vector2(16, 32), new BoundingBox(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.9f, 0.9f, 0.0f)), "grass")
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
            tiles[3, 4].SetJoinWall(joinWallSprite);
            tiles[3, 5].SetRightWall(rightWallSprite);
            tiles[3, 6].SetRightWall(rightWallSprite);
            tiles[3, 7].SetLeftWall(leftWallSprite);
            tiles[4, 4].SetLeftWall(leftWallSprite);
            tiles[5, 4].SetLeftWall(leftWallSprite);
            tiles[5, 7].SetLeftWall(leftWallSprite);
            tiles[6, 4].SetRightWall(rightWallSprite);
            tiles[6, 5].SetRightWall(rightWallSprite);
            tiles[6, 6].SetRightWall(rightWallSprite);

            _map = new Map((short)_mapSize.X, (short)_mapSize.Y, tiles);

            var human = new Human("human", humanSprite, new Coordinate(10, 10));
            _map.AddEntity(human);
            _map.GetTile(human.Coordinate).IsBlocked = true;

            var zombie = new Zombie("zombie", zombieSprite, new Coordinate(16, 16));
            _map.AddEntity(zombie);
            _map.GetTile(zombie.Coordinate).IsBlocked = true;

            var light = new PhantomLight("light", new Coordinate(7, 9), new Light(new Coordinate(7, 9), Color.White, 10));
            _map.AddEntity(light);

            var light2 = new PhantomLight("light", new Coordinate(4, 5), new Light(new Coordinate(4, 5), Color.Red, 10));
            _map.AddEntity(light2);

            _lightMap = new LightMap(_map, new Color(0.1f, 0.1f, 0.4f));
            _pathfindingMap = new PathfindingMap(_map);

            GameState.Map = _map;
            GameState.PathfindingMap = _pathfindingMap;

            _drawingManager.RegisterProvider(_map);

            Console.Initialize(_spriteBatch, font, 10);
            Console.WriteLine("THE CONSOLE");
            _uiManager.RegisterProvider(Console.DrawingProvider);

            FrameRater.Initialize(_spriteBatch, font);
            _uiManager.RegisterProvider(FrameRater.DrawingProvider);
        }

        private AnimatedSprite BuildHumanSprite()
        {
            var humanTexture = Texture2D.FromStream(GraphicsDevice, TitleContainer.OpenStream("Content/SpriteSheets/ethereal.png"));

            var humanSpriteSheet = new SpriteSheet("ethereal", humanTexture);
            humanSpriteSheet.AddFrame("standingDown", new Rectangle(0, 0, 32, 48));
            humanSpriteSheet.AddFrame("standingDownLeft", new Rectangle(32, 0, 32, 48));
            humanSpriteSheet.AddFrame("standingLeft", new Rectangle(64, 0, 32, 48));
            humanSpriteSheet.AddFrame("standingUpLeft", new Rectangle(96, 0, 32, 48));
            humanSpriteSheet.AddFrame("standingUp", new Rectangle(128, 0, 32, 48));
            humanSpriteSheet.AddFrame("standingUpRight", new Rectangle(160, 0, 32, 48));
            humanSpriteSheet.AddFrame("standingRight", new Rectangle(192, 0, 32, 48));
            humanSpriteSheet.AddFrame("standingDownRight", new Rectangle(224, 0, 32, 48));

            var humanAnimationList = new AnimationList();
            var standingUp = new Animation(AnimationType.RunOnce);
            standingUp.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("standingUp"), 1.0));
            var standingDown = new Animation(AnimationType.RunOnce);
            standingDown.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("standingDown"), 1.0));
            var standingDownLeft = new Animation(AnimationType.RunOnce);
            standingDownLeft.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("standingDownLeft"), 1.0));
            var standingLeft = new Animation(AnimationType.RunOnce);
            standingLeft.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("standingLeft"), 1.0));
            var standingUpLeft = new Animation(AnimationType.RunOnce);
            standingUpLeft.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("standingUpLeft"), 1.0));
            var standingUpRight = new Animation(AnimationType.RunOnce);
            standingUpRight.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("standingUpRight"), 1.0));
            var standingRight = new Animation(AnimationType.RunOnce);
            standingRight.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("standingRight"), 1.0));
            var standingDownRight = new Animation(AnimationType.RunOnce);
            standingDownRight.AddFrame(new AnimationFrame(humanSpriteSheet.GetFrameRectangle("standingDownRight"), 1.0));

            humanAnimationList.Add("standingDown", standingDown);
            humanAnimationList.Add("standingUp", standingUp);
            humanAnimationList.Add("standingDownLeft", standingDownLeft);
            humanAnimationList.Add("standingLeft", standingLeft);
            humanAnimationList.Add("standingUpLeft", standingUpLeft);
            humanAnimationList.Add("standingUpRight", standingUpRight);
            humanAnimationList.Add("standingRight", standingRight);
            humanAnimationList.Add("standingDownRight", standingDownRight);

            var humanSprite = new AnimatedSprite("human", humanSpriteSheet, new Vector2(16, 40), new BoundingBox(new Vector3(0.2f, 0.2f, 0.2f), new Vector3(0.8f, 0.8f, 0.8f)), humanAnimationList);

            return humanSprite;
        }

        private AnimatedSprite BuildZombieSprite()
        {
            var zombieTexture = Texture2D.FromStream(GraphicsDevice, TitleContainer.OpenStream("Content/SpriteSheets/zombie.png"));

            var zombieSpriteSheet = new SpriteSheet("zombie", zombieTexture);
            zombieSpriteSheet.AddFrame("standingDown", new Rectangle(0, 0, 32, 48));
            zombieSpriteSheet.AddFrame("standingDownLeft", new Rectangle(32, 0, 32, 48));
            zombieSpriteSheet.AddFrame("standingLeft", new Rectangle(64, 0, 32, 48));
            zombieSpriteSheet.AddFrame("standingUpLeft", new Rectangle(96, 0, 32, 48));
            zombieSpriteSheet.AddFrame("standingUp", new Rectangle(128, 0, 32, 48));
            zombieSpriteSheet.AddFrame("standingUpRight", new Rectangle(160, 0, 32, 48));
            zombieSpriteSheet.AddFrame("standingRight", new Rectangle(192, 0, 32, 48));
            zombieSpriteSheet.AddFrame("standingDownRight", new Rectangle(224, 0, 32, 48));

            var humanAnimationList = new AnimationList();
            var standingUp = new Animation(AnimationType.RunOnce);
            standingUp.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("standingUp"), 1.0));
            var standingDown = new Animation(AnimationType.RunOnce);
            standingDown.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("standingDown"), 1.0));
            var standingDownLeft = new Animation(AnimationType.RunOnce);
            standingDownLeft.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("standingDownLeft"), 1.0));
            var standingLeft = new Animation(AnimationType.RunOnce);
            standingLeft.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("standingLeft"), 1.0));
            var standingUpLeft = new Animation(AnimationType.RunOnce);
            standingUpLeft.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("standingUpLeft"), 1.0));
            var standingUpRight = new Animation(AnimationType.RunOnce);
            standingUpRight.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("standingUpRight"), 1.0));
            var standingRight = new Animation(AnimationType.RunOnce);
            standingRight.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("standingRight"), 1.0));
            var standingDownRight = new Animation(AnimationType.RunOnce);
            standingDownRight.AddFrame(new AnimationFrame(zombieSpriteSheet.GetFrameRectangle("standingDownRight"), 1.0));

            humanAnimationList.Add("standingDown", standingDown);
            humanAnimationList.Add("standingUp", standingUp);
            humanAnimationList.Add("standingDownLeft", standingDownLeft);
            humanAnimationList.Add("standingLeft", standingLeft);
            humanAnimationList.Add("standingUpLeft", standingUpLeft);
            humanAnimationList.Add("standingUpRight", standingUpRight);
            humanAnimationList.Add("standingRight", standingRight);
            humanAnimationList.Add("standingDownRight", standingDownRight);

            var zombieSprite = new AnimatedSprite("human", zombieSpriteSheet, new Vector2(16, 40), new BoundingBox(new Vector3(0.2f, 0.2f, 0.2f), new Vector3(0.8f, 0.8f, 0.8f)), humanAnimationList);

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
            FrameRater.NewUpdate(gameTime.TotalGameTime);

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
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            FrameRater.NewFrame(gameTime.TotalGameTime);

            _virtualScreen.BeginCapture();
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            _drawingManager.Draw(_spriteBatch);
            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            _uiManager.Draw(_spriteBatch);
            _spriteBatch.End();

            _virtualScreen.EndCapture();

            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            _virtualScreen.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
