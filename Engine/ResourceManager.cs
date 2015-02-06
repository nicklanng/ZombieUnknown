using System.Collections.Generic;
using Engine.Sprites;

namespace Engine
{
    public static class ResourceManager
    {
        private static readonly Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();

        public static void RegisterSprite(Sprite sprite)
        {
            Sprites.Add(sprite.Name, sprite);
        }

        public static Sprite GetSprite(string spriteName)
        {
            return Sprites[spriteName];
        }
    }
}
