using System;
using Engine.Maps;
using Engine.Sprites;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Drawing
{
    public class UIRequest
    {
        private readonly Sprite _sprite;
        private readonly Coordinate _screenPosition;
        private readonly float _depth;
        
        public UIRequest(Sprite sprite, Coordinate screenPosition, float depth)
        {
            _sprite = sprite;
            _screenPosition = screenPosition;
            _depth = depth;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _sprite.Draw(spriteBatch, _screenPosition, _depth);
        }
    }
}
