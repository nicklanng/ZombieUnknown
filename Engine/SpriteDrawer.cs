using Engine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    public static class SpriteDrawer
    {
        private static ICamera _camera;
        private static SpriteBatch _spriteBatch;

        public static void Initialize(ICamera camera, SpriteBatch spriteBatch)
        {
            _camera = camera;
            _spriteBatch = spriteBatch;
        }

        public static void Draw(Sprite sprite, Vector2 mapPosition, Color light)
        {
            Vector2 screenCoordinates;
            if (_camera.GetScreenCoordinates(mapPosition, out screenCoordinates))
            {
                sprite.Draw(_spriteBatch, screenCoordinates, light);
            }
        }

        
    }
}
