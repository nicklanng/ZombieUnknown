using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Engine
{
    public class Cursor
    {
        private readonly Map _map;
        private readonly Sprite _frontCursor;
        private readonly Sprite _backCursor;

        public bool IsOnMap { get; private set; }
        public Vector2 MapPosition { get; private set; }

        public Cursor(Map map, Sprite frontCursor, Sprite backCursor)
        {
            _map = map;
            _frontCursor = frontCursor;
            _backCursor = backCursor;
        }

        public void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            var screenCoordinates = new Vector2(mouseState.X, mouseState.Y);

            Vector2 mapPosition;
            IsOnMap = _map.GetMapCoordinates(screenCoordinates, out mapPosition);
            MapPosition = mapPosition;
        }

        public void DrawFrontSprite(SpriteBatch spriteBatch, Vector2 position)
        {
            _frontCursor.Draw(spriteBatch, position);
        }

        public void DrawBackSprite(SpriteBatch spriteBatch, Vector2 position)
        {
            _backCursor.Draw(spriteBatch, position);
        }
    }
}
