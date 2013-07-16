using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine
{
    public class Manual2dCamera
    {
        public Vector2 Size { get; set; }

        public float MoveSpeed { get; set; }

        public Vector2 Position { get; set; }

        public Manual2dCamera(Vector2 size, float moveSpeed)
        {
            Size = size;
            MoveSpeed = moveSpeed;
        }

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

        private void Translate(Vector2 vector2)
        {
            Position += vector2;
        }
    }
}
