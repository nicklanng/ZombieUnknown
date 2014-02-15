using System.Collections.Generic;
using Engine.Drawing;
using Engine.Maps;
using Engine.Sprites;
using Microsoft.Xna.Framework;

namespace Engine.Entities
{
    public abstract class Entity : IDrawingProvider
    {
        protected readonly Sprite Sprite;

        public Coordinate Coordinate { get; set; }
        public string Name { get; private set; }
        public short ZIndex { get; protected set; }

        protected Entity(string name, Sprite sprite, Coordinate coordinate)
        {
            Name = name;
            Sprite = sprite;
            Coordinate = coordinate;

            ZIndex = 0;
        }

        public virtual void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
        }

        public virtual IEnumerable<DrawingRequest> GetDrawings()
        {
            yield return new DrawingRequest(Sprite, Coordinate.ToVector2(), Color.White);
        }
    }
}
