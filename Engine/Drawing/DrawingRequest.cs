using Engine.Maps;
using Engine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Drawing
{
    public class DrawingRequest
    {
        private readonly Color _light;
        private float _depth;
        public Sprite Sprite { get; private set; }
        public Vector2 MapPosition { get; private set; }
        public Vector2 ScreenCoordinates { get; set; }

        public DrawingRequest(Sprite sprite, Vector2 mapPosition, Color light)
        {
            _light = light;
            _depth = (MapPosition.X + MapPosition.Y) / (GameState.Map.Width + GameState.Map.Height);
            Sprite = sprite;
            MapPosition = mapPosition;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch, ScreenCoordinates, _light, _depth);
        }
    }
}
