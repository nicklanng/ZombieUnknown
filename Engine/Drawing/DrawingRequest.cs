using System;
using Engine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Drawing
{
    public class DrawingRequest : IComparable<DrawingRequest>
    {
        private readonly Color _light;
        public Sprite Sprite { get; private set; }
        public Vector2 MapPosition { get; private set; }
        public Vector2 ScreenCoordinates { get; set; }
        public DrawingLevel DrawingLevel { get; private set; }

        public DrawingRequest(Sprite sprite, Vector2 mapPosition, Color light, DrawingLevel drawingLevel)
        {
            _light = light;
            Sprite = sprite;
            MapPosition = mapPosition;
            DrawingLevel = drawingLevel;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch, ScreenCoordinates, _light);
        }

        public int CompareTo(DrawingRequest other)
        {
            // floors go first
            if (DrawingLevel == DrawingLevel.Floor && other.DrawingLevel != DrawingLevel.Floor)
            {
                return -1;
            }

            if (other.DrawingLevel == DrawingLevel.Floor && DrawingLevel != DrawingLevel.Floor)
            {
                return 1;
            }

            // higher up tiles draw first
            var thisOrderValue = MapPosition.X + MapPosition.Y;
            var otherOrderValue = other.MapPosition.X + other.MapPosition.Y;

            if (thisOrderValue < otherOrderValue)
            {
                return -1;
            }

            if (thisOrderValue > otherOrderValue)
            {
                return 1;
            }

            // if drawings are on the same tile then use drawing levels to decide
            if (DrawingLevel < other.DrawingLevel)
            {
                return -1;
            }

            if (DrawingLevel > other.DrawingLevel)
            {
                return 1;
            }

            return 0;
        }
    }
}
