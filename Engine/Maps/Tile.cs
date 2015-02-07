using System.Collections.Generic;
using System.Linq;
using Engine.Drawing;
using Engine.Entities;
using Engine.Sprites;
using Microsoft.Xna.Framework;

namespace Engine.Maps
{
    public class Tile : IDrawingProvider
    {
        private List<Entity> _entities;

        private Sprite _leftWallSprite;
        private Sprite _rightWallSprite;
        private Sprite _wallJoinSprite;
        private readonly Sprite _floorSprite;

        public bool IsBlocked { get; set; }
        public Color Light { get; set; }
        public Vector2 Position { get; private set; }
        public PhysicalEntity PhysicalEntity { get; private set; }

        public bool HasLeftWall
        {
            get { return _leftWallSprite != null; }
        }

        public bool HasRightWall
        {
            get { return _rightWallSprite != null; }
        }

        public bool HasFloor
        {
            get { return _floorSprite != null; }
        }

        public Tile(Vector2 position, Sprite floorSprite, Sprite leftWall, Sprite rightWall, Sprite wallJoinSprite)
        {
            Position = position;
            _floorSprite = floorSprite;
            _leftWallSprite = leftWall;
            _rightWallSprite = rightWall;
            _wallJoinSprite = wallJoinSprite;

            _entities = new List<Entity>();
            Light = Color.White;
        }

        public void Update(GameTime gameTime)
        {
            if (_leftWallSprite != null) _leftWallSprite.Update();
            if (_rightWallSprite != null) _rightWallSprite.Update();
            if (_wallJoinSprite != null) _wallJoinSprite.Update();
            if (_floorSprite != null) _floorSprite.Update();

            _entities.ForEach(x => x.Update());
        }

        public void AddEntity(Entity entity)
        {
            var moveableEntity = entity as PhysicalEntity;
            if (moveableEntity != null)
            {
                PhysicalEntity = moveableEntity;
            }

            _entities.Add(entity);
            
            _entities = new List<Entity>(_entities.OrderBy(x => x.ZIndex));
        }

        public void RemoveEntity(Entity entity)
        {
            _entities.Remove(entity);
        }

        public IEnumerable<DrawingRequest> GetDrawings()
        {
            if (_floorSprite != null)
            {
                yield return new DrawingRequest(_floorSprite, Position, Light, 0);
            }

            if (_leftWallSprite != null)
            {
                yield return new DrawingRequest(_leftWallSprite, Position, Light, new Vector2(0, -0.50001f));
            }

            if (_rightWallSprite != null)
            {
                yield return new DrawingRequest(_rightWallSprite, Position, Light, new Vector2(0, -0.50001f));
            }

            if (_wallJoinSprite != null)
            {
                yield return new DrawingRequest(_wallJoinSprite, Position, Light, new Vector2(0, -0.500002f));
            }
        }

        public void SetLeftWall(StaticSprite leftWallSprite)
        {
            _leftWallSprite = leftWallSprite;
        }

        public void SetRightWall(StaticSprite rightWallSprite)
        {
            _rightWallSprite = rightWallSprite;
        }

        public void SetJoinWall(StaticSprite joinWallSprite)
        {
            _wallJoinSprite = joinWallSprite;
        }
    }
}
