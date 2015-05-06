using Engine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Drawing
{
    public class DrawingRequest
    {
        private readonly Color _light;
        private readonly float _depth;
        public Sprite Sprite { get; private set; }
        public Vector2 MapPosition { get; private set; }
        public Vector2 ScreenCoordinates { get; set; }

        public DrawingRequest(Sprite sprite, Vector2 mapPosition, Color light)
        {
            _light = light;
            Sprite = sprite;
            MapPosition = mapPosition;
            _depth = (MapPosition.X + MapPosition.Y) / (GameState.Map.Width + GameState.Map.Height);
        }

        public DrawingRequest(Sprite sprite, Vector2 mapPosition, Color light, float depth)
            : this(sprite, mapPosition, light)
        {
            _depth = depth;
        }

        public DrawingRequest(Sprite sprite, Vector2 mapPosition, Color light, Vector2 depthOffset)
            : this(sprite, mapPosition, light)
        {
            _depth = ((MapPosition.X + depthOffset.X) + (MapPosition.Y + depthOffset.Y)) / (GameState.Map.Width + GameState.Map.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch, ScreenCoordinates, _light, _depth);
        }
    }
}
