using System;
using System.Collections.Generic;
using Engine;
using Engine.Drawing;
using Engine.Entities;
using Engine.Maps;
using Engine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ZombieUnknown.Entities;

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

        private DrawingManager _drawingManager;

        private Vector2 _mapSize = new Vector2(3, 3);
        private SpriteBatch _spriteBatch;

        public ZombieGameMain()
        {
            _graphics = new GraphicsDeviceManager(this)
                {
                    
                    PreferredBackBufferWidth = 640,
                    PreferredBackBufferHeight = 480
                };
            Content.RootDirectory = "Content";

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

            _camera = new Camera(new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), 200, new IsometricConfiguration());

            _drawingManager = new DrawingManager(_camera);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            
            // All of the initialization stuff should be somewhere else, and probably load from data files
            var terrainTexture = Texture2D.FromStream(GraphicsDevice, TitleContainer.OpenStream("Content/SpriteSheets/xcom-forest.png"));
            var cursorTexture = Texture2D.FromStream(GraphicsDevice, TitleContainer.OpenStream("Content/SpriteSheets/cursor.png"));
            var wallsTexture = Texture2D.FromStream(GraphicsDevice, TitleContainer.OpenStream("Content/SpriteSheets/walls.png"));
            var debugIconTexture = Texture2D.FromStream(GraphicsDevice, TitleContainer.OpenStream("Content/SpriteSheets/debugIcons.png"));
            var etherealTexture = Texture2D.FromStream(GraphicsDevice, TitleContainer.OpenStream("Content/SpriteSheets/ethereal.png"));

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

            var etherealSpriteSheet = new SpriteSheet("ethereal", etherealTexture);
            etherealSpriteSheet.AddFrame("standingDown", new Rectangle(0, 0, 32, 48));
            etherealSpriteSheet.AddFrame("standingDownLeft", new Rectangle(32, 0, 32, 48));
            etherealSpriteSheet.AddFrame("standingLeft", new Rectangle(64, 0, 32, 48));
            etherealSpriteSheet.AddFrame("standingUpLeft", new Rectangle(96, 0, 32, 48));
            etherealSpriteSheet.AddFrame("standingUp", new Rectangle(128, 0, 32, 48));
            etherealSpriteSheet.AddFrame("standingUpRight", new Rectangle(160, 0, 32, 48));
            etherealSpriteSheet.AddFrame("standingRight", new Rectangle(192, 0, 32, 48));
            etherealSpriteSheet.AddFrame("standingDownRight", new Rectangle(224, 0, 32, 48));

            var leftWallSprite = new StaticSprite("left", wallSpriteSheet, new Vector2(16, 40), "left");
            var rightWallSprite = new StaticSprite("right", wallSpriteSheet, new Vector2(16, 40), "right");
            var joinWallSprite = new StaticSprite("join", wallSpriteSheet, new Vector2(16, 40), "join");

            var cursorSpriteSheet = new SpriteSheet("cursor", cursorTexture);
            cursorSpriteSheet.AddFrame("front", new Rectangle(0, 0, 32, 48));
            cursorSpriteSheet.AddFrame("back", new Rectangle(32, 0, 32, 48));
            cursorSpriteSheet.AddFrame("selectedMarker1", new Rectangle(64, 0, 32, 48));
            cursorSpriteSheet.AddFrame("selectedMarker2", new Rectangle(96, 0, 32, 48));

            var frontCursorSprite = new StaticSprite("front", cursorSpriteSheet, new Vector2(16, 40), "front");
            var backCursorSprite = new StaticSprite("back", cursorSpriteSheet, new Vector2(16, 40), "back");

            var selectedMarkerAnimationList = new AnimationList();
            var selectedMarkerAnimation = new Animation(AnimationType.Looped);
            selectedMarkerAnimation.AddFrame(new AnimationFrame(cursorSpriteSheet.GetFrameRectangle("selectedMarker1"), 0.2));
            selectedMarkerAnimation.AddFrame(new AnimationFrame(cursorSpriteSheet.GetFrameRectangle("selectedMarker2"), 0.2));
            selectedMarkerAnimationList.Add("select", selectedMarkerAnimation);

            ResourceManager.SelectMarkerSprite = new AnimatedSprite("selected", cursorSpriteSheet, new Vector2(16, 40), selectedMarkerAnimationList);

            var lightSprite = new StaticSprite("light", debugIconsSpriteSheet, new Vector2(16, 40), "light");

            var etherealAnimationList = new AnimationList();
            var standingAnimation = new Animation(AnimationType.Looped);
            standingAnimation.AddFrame(new AnimationFrame(etherealSpriteSheet.GetFrameRectangle("standingDown"), 1.0));
            standingAnimation.AddFrame(new AnimationFrame(etherealSpriteSheet.GetFrameRectangle("standingDownLeft"), 1.0));
            standingAnimation.AddFrame(new AnimationFrame(etherealSpriteSheet.GetFrameRectangle("standingLeft"), 1.0));
            standingAnimation.AddFrame(new AnimationFrame(etherealSpriteSheet.GetFrameRectangle("standingUpLeft"), 1.0));
            standingAnimation.AddFrame(new AnimationFrame(etherealSpriteSheet.GetFrameRectangle("standingUp"), 1.0));
            standingAnimation.AddFrame(new AnimationFrame(etherealSpriteSheet.GetFrameRectangle("standingUpRight"), 1.0));
            standingAnimation.AddFrame(new AnimationFrame(etherealSpriteSheet.GetFrameRectangle("standingRight"), 1.0));
            standingAnimation.AddFrame(new AnimationFrame(etherealSpriteSheet.GetFrameRectangle("standingDownRight"), 1.0));
            etherealAnimationList.Add("standing", standingAnimation);

            var etherealSprite = new AnimatedSprite("standingDown", etherealSpriteSheet, new Vector2(16, 40), etherealAnimationList);

            var terrainSprites = new List<Sprite>
                {
                    new StaticSprite("grass", terrainSpriteSheet, new Vector2(16, 32), "grass"),
                    new StaticSprite("leaves1", terrainSpriteSheet, new Vector2(16, 32), "leaves1"),
                    new StaticSprite("leaves2", terrainSpriteSheet, new Vector2(16, 32), "leaves2"),
                    new StaticSprite("water", terrainSpriteSheet, new Vector2(16, 32), "water"),
                    new StaticSprite("tallGrass1", terrainSpriteSheet, new Vector2(16, 32), "tallGrass1"),
                    new StaticSprite("tallGrass2", terrainSpriteSheet, new Vector2(16, 32), "tallGrass2"),
                    new StaticSprite("swamp1", terrainSpriteSheet, new Vector2(16, 32), "swamp1"),
                    new StaticSprite("swamp2", terrainSpriteSheet, new Vector2(16, 32), "swamp2"),
                    new StaticSprite("deadTree1", terrainSpriteSheet, new Vector2(16, 32), "deadTree1"),
                    new StaticSprite("deadTree2", terrainSpriteSheet, new Vector2(16, 32), "deadTree2"),
                    new StaticSprite("deadTree3", terrainSpriteSheet, new Vector2(16, 32), "deadTree3"),
                    new StaticSprite("deadTree4", terrainSpriteSheet, new Vector2(16, 32), "deadTree4")
                };

            var random = new Random();
            var tiles = new Tile[(int)_mapSize.X, (int)_mapSize.Y];

            tiles[0, 0] = new Tile(new Vector2(0, 0), terrainSprites[0], leftWallSprite, null, null);
            tiles[0, 1] = new Tile(new Vector2(0, 1), terrainSprites[0], leftWallSprite, null, null);
            tiles[0, 2] = new Tile(new Vector2(0, 2), terrainSprites[0], leftWallSprite, null, null);
            tiles[1, 0] = new Tile(new Vector2(1, 0), terrainSprites[0], leftWallSprite, null, null);
            tiles[1, 1] = new Tile(new Vector2(1, 1), terrainSprites[0], leftWallSprite, null, null);
            tiles[1, 2] = new Tile(new Vector2(1, 2), terrainSprites[0], leftWallSprite, null, null);
            tiles[2, 0] = new Tile(new Vector2(2, 0), terrainSprites[0], null, null, null);
            tiles[2, 1] = new Tile(new Vector2(2, 1), terrainSprites[0], null, null, null);
            tiles[2, 2] = new Tile(new Vector2(2, 2), terrainSprites[0], null, null, null);

            _map = new Map((short)_mapSize.X, (short)_mapSize.Y, tiles);
            _pathfindingMap = new PathfindingMap(_map);

            _lightMap = new LightMap(_map, new Color(0.5f, 0.5f, 0.5f));

            var human = new Human("human", etherealSprite, new Coordinate(1, 0));
            _map.AddEntity(human, human.Coordinate);

            GameState.Map = _map;
            GameState.PathfindingMap = _pathfindingMap;

            _drawingManager.RegisterProvider(_map);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            Engine.Input.Mouse.Update(gameTime);

            _camera.Update(gameTime);
            _map.Update(gameTime);

            ResourceManager.SelectMarkerSprite.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);

            _drawingManager.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
