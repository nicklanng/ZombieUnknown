using Engine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine
{
    public interface ISpriteDrawer
    {
        void Draw(Sprite sprite, Vector2 mapPosition, Color light);
    }

    class SpriteDrawer : ISpriteDrawer
    {
        private readonly Manual2dCamera _camera;
        private readonly SpriteBatch _spriteBatch;
        private readonly IIsometricConfiguration _isometricConfiguration;

        public SpriteDrawer(Manual2dCamera camera, SpriteBatch spriteBatch, IIsometricConfiguration isometricConfiguration)
        {
            _camera = camera;
            _spriteBatch = spriteBatch;
            _isometricConfiguration = isometricConfiguration;
        }

        public void Draw(Sprite sprite, Vector2 mapPosition, Color light)
        {
            Vector2 screenCoordinates;
            if (GetScreenCoordinates(mapPosition, out screenCoordinates))
            {
                sprite.Draw(_spriteBatch, screenCoordinates, light);
            }
        }

        private bool GetScreenCoordinates(Vector2 mapCoordinates, out Vector2 screenCoordinates)
        {
            screenCoordinates = Vector2.Zero;

            var xCoord = ((mapCoordinates.X + mapCoordinates.Y) * _isometricConfiguration.FloorWidth / 2) - (int)_camera.Position.X + (int)(_camera.Size.X / 2);
            if (xCoord <= 0)
            {
                if (xCoord + _isometricConfiguration.FloorWidth < 0)
                {
                    return false;
                }
            }
            if (xCoord >= _camera.Size.X)
            {
                if (xCoord - _isometricConfiguration.FloorWidth > _camera.Size.X)
                {
                    return false;
                }
            }

            var yCoord = ((mapCoordinates.X - mapCoordinates.Y) * _isometricConfiguration.FloorHeight / 2) - (int)_camera.Position.Y + (int)(_camera.Size.Y / 2);
            if (yCoord <= 0)
            {
                if (yCoord + _isometricConfiguration.TileHeight < 0)
                {
                    return false;
                }
            }
            if (yCoord >= _camera.Size.Y)
            {
                if (yCoord - _isometricConfiguration.TileHeight > _camera.Size.Y)
                {
                    return false;
                }
            }

            screenCoordinates = new Vector2(xCoord, yCoord);
            return true;
        }
    }
}
