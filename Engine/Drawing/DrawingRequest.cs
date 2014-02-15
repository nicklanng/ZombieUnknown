using System;
using System.Collections.Generic;
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
        public BoundingBox WorldBoundingBox { get; private set; }
        public List<DrawingRequest> SpritesBehind { get; private set; }
        public int IsoDepth { get; set; }
        public bool HasBeenVisited;

        public DrawingRequest(Sprite sprite, Vector2 mapPosition, Color light)
        {
            _light = light;
            Sprite = sprite;
            MapPosition = mapPosition;

            SpritesBehind = new List<DrawingRequest>();

            WorldBoundingBox = new BoundingBox(
                Sprite.LocalBoundingBox.Min + new Vector3(MapPosition.X, MapPosition.Y, 0),
                Sprite.LocalBoundingBox.Max + new Vector3(MapPosition.X, MapPosition.Y, 0)
                );
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch, ScreenCoordinates, _light);
        }

        public int CompareTo(DrawingRequest other)
        {
            if (IsoDepth < other.IsoDepth)
            {
                return -1;
            }

            if (IsoDepth > other.IsoDepth)
            {
                return 1;
            }

            return 0;
        }
    }
}
