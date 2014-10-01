using System;
using Engine.Maps;
using Engine.Sprites;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Drawing
{
    public class UIRequest : IComparable<UIRequest>
    {
        private readonly Sprite _sprite;
        private readonly Coordinate _screenPosition;

        public int ZLevel { get; private set; }

        public UIRequest(Sprite sprite, Coordinate screenPosition, int zLevel)
        {
            ZLevel = zLevel;
            _sprite = sprite;
            _screenPosition = screenPosition;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _sprite.Draw(spriteBatch, _screenPosition);
        }

        public int CompareTo(UIRequest other)
        {
            if (ZLevel < other.ZLevel)
            {
                return -1;
            }

            if (ZLevel > other.ZLevel)
            {
                return 1;
            }

            return 0;
        }


    }
}
