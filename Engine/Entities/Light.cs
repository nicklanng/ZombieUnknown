using System.Collections.Generic;
using Engine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Entities
{
    public class Light : Entity
    {
        private readonly int _mapSize;

        public Color Color { get; private set; }

        public short Range { get; private set; }

        public float[,] IntensityMap { get; private set; }

        public byte[,] VisiblityMap { get; private set; }

        public Light(string name, Sprite lightSprite, Color color, short range)
            : base(name, lightSprite)
        {
            Color = color;
            Range = range;

            _mapSize = 2 * Range + 1;

            IntensityMap = new float[_mapSize, _mapSize];
            VisiblityMap = new byte[_mapSize, _mapSize];

            GenerateIntensityMap();
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 position, Color light)
        {
            Sprite.Draw(spriteBatch, position, Color);
        }

        public void GenerateVisibiltyMap(List<Line> walls)
        {
            foreach (var wall in walls)
            {
                System.Console.WriteLine("Wall: {0} {1} - {2} {3}", wall.Start.X, wall.Start.Y, wall.End.X, wall.End.Y);
            }
        }

        private void GenerateIntensityMap()
        {
            var source = new Vector2(Range, Range);

            for (var x = 0; x < _mapSize; x++)
            {
                for (var y = 0; y < _mapSize; y++)
                {
                    var tile = new Vector2(x, y);

                    if (tile == source)
                    {
                        IntensityMap[x, y] = 1;
                    }
                    else
                    {
                        var distance = Vector2.Distance(source, tile);

                        var scalar = 1 - (distance / Range);
                        if (scalar < 0) scalar = 0;

                        IntensityMap[x, y] = scalar;
                    }
                }
            }
        }
    }
}
