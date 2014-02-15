using Engine.Maps;

namespace Engine.Entities
{
    public class PhantomLight : Entity, ILightSource
    {
        public Light Light { get; private set; }

        public PhantomLight(string name, Coordinate coordinate, Light light) 
            : base(name, coordinate)
        {
            Light = light;
        }
    }
}
