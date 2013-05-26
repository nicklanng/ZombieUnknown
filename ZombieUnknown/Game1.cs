using System;
using System.Collections.Generic;
using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ZombieUnknown
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Map _map;
        private Camera _camera;
        private Cursor _cursor;

        private Vector2 _mapSize = new Vector2(100, 100);

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
                {
                    PreferredBackBufferHeight = 1080,
                    PreferredBackBufferWidth = 1920
                };
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _camera = new Camera(new Vector2(_graphics.PreferredBackBufferWidth / Constants.ZoomFactor, _graphics.PreferredBackBufferHeight / Constants.ZoomFactor), 200.0f);

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // All of the initialization stuff should be somewhere else, and probably load from data files
            var terrainTexture = Texture2D.FromStream(GraphicsDevice, TitleContainer.OpenStream("Content/SpriteSheets/xcom-forest.png"));
            var cursorTexture = Texture2D.FromStream(GraphicsDevice, TitleContainer.OpenStream("Content/SpriteSheets/cursor.png"));
            var wallsTexture = Texture2D.FromStream(GraphicsDevice, TitleContainer.OpenStream("Content/SpriteSheets/walls.png"));
            var debugIconTexture = Texture2D.FromStream(GraphicsDevice, TitleContainer.OpenStream("Content/SpriteSheets/debugIcons.png"));

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

            var leftWallSprite = new StaticSprite("left", wallSpriteSheet, "left", new Vector2(16, 40));
            var rightWallSprite = new StaticSprite("right", wallSpriteSheet, "right", new Vector2(16, 40));
            var joinWallSprite = new StaticSprite("join", wallSpriteSheet, "join", new Vector2(16, 40));

            var cursorSpriteSheet = new SpriteSheet("cursor", cursorTexture);
            cursorSpriteSheet.AddFrame("front", new Rectangle(0, 0, 32, 48));
            cursorSpriteSheet.AddFrame("back", new Rectangle(32, 0, 32, 48));

            var frontCursorSprite = new StaticSprite("front", cursorSpriteSheet, "front", new Vector2(16, 40));
            var backCursorSprite = new StaticSprite("back", cursorSpriteSheet, "back", new Vector2(16, 40));

            var lightSprite = new StaticSprite("light", debugIconsSpriteSheet, "light", new Vector2(16, 40));

            var terrainSprites = new List<Sprite>
                {
                    new StaticSprite("grass", terrainSpriteSheet, "grass", new Vector2(16, 32)),
                    new StaticSprite("leaves1", terrainSpriteSheet, "leaves1", new Vector2(16, 32)),
                    new StaticSprite("leaves2", terrainSpriteSheet, "leaves2", new Vector2(16, 32)),
                    new StaticSprite("water", terrainSpriteSheet, "water", new Vector2(16, 32)),
                    new StaticSprite("tallGrass1", terrainSpriteSheet, "tallGrass1", new Vector2(16, 32)),
                    new StaticSprite("tallGrass2", terrainSpriteSheet, "tallGrass2", new Vector2(16, 32)),
                    new StaticSprite("swamp1", terrainSpriteSheet, "swamp1", new Vector2(16, 32)),
                    new StaticSprite("swamp2", terrainSpriteSheet, "swamp2", new Vector2(16, 32)),
                    new StaticSprite("deadTree1", terrainSpriteSheet, "deadTree1", new Vector2(16, 32)),
                    new StaticSprite("deadTree2", terrainSpriteSheet, "deadTree2", new Vector2(16, 32)),
                    new StaticSprite("deadTree3", terrainSpriteSheet, "deadTree3", new Vector2(16, 32)),
                    new StaticSprite("deadTree4", terrainSpriteSheet, "deadTree4", new Vector2(16, 32))
                };

            var random = new Random();
            var tiles = new Tile[(int)_mapSize.X, (int)_mapSize.Y];
            for (var x = 0; x < _mapSize.X; x++)
            {
                for (var y = 0; y < _mapSize.Y; y++)
                {
                    var interestingTile = random.NextDouble();
                    var index = 0;

                    if (interestingTile > 0.7)
                    {
                        index = random.Next(terrainSprites.Count - 1) + 1;
                    }

                    Sprite leftWall = null;
                    if (random.NextDouble() > 0.9)
                    {
                        leftWall = leftWallSprite;
                    }

                    Sprite rightWall = null;
                    if (random.NextDouble() > 0.9)
                    {
                        rightWall = rightWallSprite;
                    }

                    Sprite joinWall = null;
                    if (leftWall != null && rightWall != null)
                    {
                        joinWall = joinWallSprite;
                    }

                    tiles[x, y] = new Tile(new Vector2(x, y), terrainSprites[index], leftWall, rightWall, joinWall);
                }
            }
            _map = new Map((short)_mapSize.X, (short)_mapSize.Y, tiles, new Color(0.1f, 0.1f, 0.1f), _camera);
            _map.AddLight(new Vector2(4, 7), new Light(lightSprite, Color.Gray, 7));
            _map.AddLight(new Vector2(8, 11), new Light(lightSprite, Color.Red, 10));
            _map.AddLight(new Vector2(15, 4), new Light(lightSprite, new Color(0.2f, 0.2f, 1.0f), 11));
            _map.AddLight(new Vector2(37, 16), new Light(lightSprite, Color.White, 12));

            _cursor = new Cursor(_map, frontCursorSprite, backCursorSprite);
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

            _camera.Update(gameTime);
            _cursor.Update(gameTime);
            _map.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            var now = DateTime.Now;

            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);

            _map.Draw(_spriteBatch, _cursor);

            _spriteBatch.End();

            base.Draw(gameTime);

            var timeTaken = DateTime.Now - now;
            Console.WriteLine("TimeToDraw: " + timeTaken.TotalSeconds);
        }
    }
}
