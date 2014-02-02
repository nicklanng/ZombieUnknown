using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine
{
    public interface ICamera
    {
        Vector2 Size { get; set; }
        float MoveSpeed { get; set; }
        Vector2 Position { get; set; }
        void Update(GameTime gameTime);
        void Translate(Vector2 vector2);
        bool GetScreenCoordinates(Vector2 mapCoordinates, out Vector2 screenCoordinates);
        Vector2 GetMapCoordinates(Vector2 screenCoordinates);
    }

    public class Camera : ICamera
    {
        private IIsometricConfiguration _isometricConfiguration;

        public Camera(Vector2 size, int moveSpeed, IIsometricConfiguration isometricConfiguration)
        {
            Size = size;
            MoveSpeed = moveSpeed;
            _isometricConfiguration = isometricConfiguration;
        }

        public Vector2 Size { get; set; }

        public float MoveSpeed { get; set; }

        public Vector2 Position { get; set; }

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Translate(-Vector2.UnitY * (float)gameTime.ElapsedGameTime.TotalSeconds * MoveSpeed);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Translate(Vector2.UnitY * (float)gameTime.ElapsedGameTime.TotalSeconds * MoveSpeed);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Translate(-Vector2.UnitX * (float)gameTime.ElapsedGameTime.TotalSeconds * MoveSpeed);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Translate(Vector2.UnitX * (float)gameTime.ElapsedGameTime.TotalSeconds * MoveSpeed);
            }
        }

        public void Translate(Vector2 vector2)
        {
            Position += vector2;
        }

        public bool GetScreenCoordinates(Vector2 mapCoordinates, out Vector2 screenCoordinates)
        {
            screenCoordinates = Vector2.Zero;

            var xCoord = ((mapCoordinates.X + mapCoordinates.Y) * _isometricConfiguration.FloorWidth / 2) - (int)Position.X + (int)(Size.X / 2);
            if (xCoord <= 0)
            {
                if (xCoord + _isometricConfiguration.FloorWidth < 0)
                {
                    return false;
                }
            }
            if (xCoord >= Size.X)
            {
                if (xCoord - _isometricConfiguration.FloorWidth > Size.X)
                {
                    return false;
                }
            }

            var yCoord = ((mapCoordinates.X - mapCoordinates.Y) * _isometricConfiguration.FloorHeight / 2) - (int)Position.Y + (int)(Size.Y / 2);
            if (yCoord <= 0)
            {
                if (yCoord + _isometricConfiguration.TileHeight < 0)
                {
                    return false;
                }
            }
            if (yCoord >= Size.Y)
            {
                if (yCoord - _isometricConfiguration.TileHeight > Size.Y)
                {
                    return false;
                }
            }

            screenCoordinates = new Vector2(xCoord, yCoord);
            return true;
        }

        public Vector2 GetMapCoordinates(Vector2 screenCoordinates)
        {
            const int mouseVerticalOffset = 10;

            var xStripped = screenCoordinates.X - (int)(Size.X / 2) + (int)Position.X + _isometricConfiguration.FloorWidth / 2;
            var yStripped = (screenCoordinates.Y - (int)(Size.Y / 2) + (int)Position.Y) + mouseVerticalOffset;

            var isoX = yStripped / (_isometricConfiguration.FloorHeight) + xStripped / (_isometricConfiguration.FloorWidth);
            var isoY = -(yStripped / (_isometricConfiguration.FloorHeight) - xStripped / (_isometricConfiguration.FloorWidth));

            return new Vector2((int)isoX, (int)isoY);
        }
    }
}
