using Microsoft.Xna.Framework;

namespace Engine.Entities
{
    public abstract class Entity
    {
        private Vector2 _mapPosition;

        public string Name { get; private set; }
        public short ZIndex { get; protected set; }
        protected Color Light { get; set; }

        public Vector2 MapPosition
        {
            get { return _mapPosition; }
            set
            {
                _mapPosition = value;
                var parentTile = GameState.Map.GetTile(_mapPosition);
                if (parentTile != null) Light = parentTile.Light;
            }
        }


        protected Entity(string name, Vector2 mapPosition)
        {
            Name = name;
            MapPosition = mapPosition;

            ZIndex = 0;
        }

        public virtual void Update(GameTime gameTime) { }
    }
}
