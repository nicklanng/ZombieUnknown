using Engine.Maps;
using Microsoft.Xna.Framework;

namespace Engine.Entities
{
    public class PhantomLight : PhysicalEntity, ILightSource
    {
        public Light Light { get; private set; }

        public PhantomLight(string name, Coordinate mapPosition, Color color, short range) 
            : base(name, mapPosition)
        {
            Light = new Light(mapPosition, color, range);
        }
    }
}
