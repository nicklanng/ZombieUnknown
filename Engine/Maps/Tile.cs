using System.Collections.Generic;
using System.Linq;
using Engine.Entities;
using Engine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Maps
{
    public class Tile
    {
        private List<Entity> _entities;

        private readonly Sprite _leftWallSprite;
        private readonly Sprite _rightWallSprite;
        private readonly Sprite _wallJoinSprite;
        private readonly ISpriteDrawer _spriteDrawer;
        private readonly Sprite _floorSprite;

        public Color Light { get; set; }
        public Vector2 Position { get; private set; }
        public MoveableEntity MoveableEntity { get; private set; }

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

        public Tile(Vector2 position, Sprite floorSprite, Sprite leftWall, Sprite rightWall, Sprite wallJoinSprite, ISpriteDrawer spriteDrawer)
        {
            Position = position;
            _floorSprite = floorSprite;
            _leftWallSprite = leftWall;
            _rightWallSprite = rightWall;
            _wallJoinSprite = wallJoinSprite;
            _spriteDrawer = spriteDrawer;

            _entities = new List<Entity>();
            Light = Color.White;
        }

        public void Update(GameTime gameTime)
        {
            if (_leftWallSprite != null) _leftWallSprite.Update(gameTime);
            if (_rightWallSprite != null) _rightWallSprite.Update(gameTime);
            if (_wallJoinSprite != null) _wallJoinSprite.Update(gameTime);
            if (_floorSprite != null) _floorSprite.Update(gameTime);

            _entities.ForEach(x => x.Update(gameTime));
        }

        public void DrawWalls()
        {
            if (_leftWallSprite != null) _spriteDrawer.Draw(_leftWallSprite, Position, Light);
            if (_rightWallSprite != null) _spriteDrawer.Draw(_rightWallSprite, Position, Light);
            if (_wallJoinSprite != null) _spriteDrawer.Draw(_wallJoinSprite, Position, Light);
        }

        public void DrawFloor()
        {
            if (_floorSprite != null) _spriteDrawer.Draw(_floorSprite, Position, Light);
        }

        public void DrawEntities()
        {
            foreach (var entity in _entities)
            {
                entity.Draw(Light);
            }
        }

        public void AddEntity(Entity entity)
        {
            var moveableEntity = entity as MoveableEntity;
            if (moveableEntity != null)
            {
                MoveableEntity = moveableEntity;
            }

            entity.MapPosition = Position;
            _entities.Add(entity);
            
            _entities = new List<Entity>(_entities.OrderBy(x => x.ZIndex));
        }

        public void RemoveEntity(Entity entity)
        {
            _entities.Remove(entity);
        }
    }
}
