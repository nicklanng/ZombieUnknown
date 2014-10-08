using System.IO;
using Engine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace Engine.Serialization
{
    public static class SpriteSheetLoader
    {
        public static SpriteSheet FromPath(string path)
        {
            var texturePath = path + ".png";
            var jsonPath = path + ".json";

            var texture = Texture2D.FromStream(GameState.GraphicsDevice, TitleContainer.OpenStream(texturePath));

            var stream = TitleContainer.OpenStream(jsonPath);
            var streamReader = new StreamReader(stream);
            var jsonSerializer = new JsonSerializer();
            var jsonTextReader = new JsonTextReader(streamReader);
            var spriteSheet = jsonSerializer.Deserialize<SerializableSpriteSheet>(jsonTextReader);

            return spriteSheet.ToSpriteSheet(texture);
        }
    }

    internal class SerializableSpriteSheet
    {
        public SerializableFrame[] Frames;

        public SpriteSheet ToSpriteSheet(Texture2D texture)
        {
            var spriteSheet = new SpriteSheet("wvr", texture);
            foreach (var serializableFrame in Frames)
            {
                spriteSheet.AddFrame(serializableFrame.Name, new Rectangle(serializableFrame.X, serializableFrame.Y, serializableFrame.Width, serializableFrame.Height));
            }
            return spriteSheet;
        }
    }

    internal struct SerializableFrame
    {
        public string Name;
        public int X;
        public int Y;
        public int Width;
        public int Height;
    }
}
