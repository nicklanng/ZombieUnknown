using Engine.Maps;

namespace Engine.Entities
{
    public class PhantomLight : Entity, ILightSource
    {
        public Light Light { get; private set; }

        public PhantomLight(string name, Coordinate mapPosition, Light light) 
            : base(name, mapPosition)
        {
            Light = light;
        }
    }
}
