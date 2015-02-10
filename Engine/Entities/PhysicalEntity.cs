using Microsoft.Xna.Framework;

namespace Engine.Entities
{
    public abstract class PhysicalEntity : Entity
    {
        protected string CurrentAnimationType;

        protected bool IsStatic = true;

        public Vector2 MapPosition { get; set; }

        protected PhysicalEntity(string name, Vector2 mapPosition)
            : base(name)
        {
            MapPosition = mapPosition;
        }
        
        public virtual AccessPosition[] AccessPositions
        {
            get { return new AccessPosition[0]; }
        }
    }
}
