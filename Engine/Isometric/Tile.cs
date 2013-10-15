using System.Collections.Generic;
using System.Linq;
using Engine.Isometric.Entities;
using Engine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Isometric
{
    public class Tile
    {
        private List<Entity> _entities;

        private readonly Sprite _leftWallSprite;
        private readonly Sprite _rightWallSprite;
        private readonly Sprite _wallJoinSprite;
        private readonly Sprite _floorSprite;

        public Color Light { get; set; }
        public Vector2 Position { get; private set; }
        public MoveableEntity MoveableEntity { get; private set; }

        public bool HasLeftWall
        {
            get
            {
                return _leftWallSprite != null;
            }
        }
        public bool HasRightWall
        {
            get
            {
                return _rightWallSprite != null;
            }
        }
        public bool HasFloor
        {
            get
            {
                return _floorSprite != null;
            }
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
            if (_leftWallSprite != null) _leftWallSprite.Update(gameTime);
            if (_rightWallSprite != null) _rightWallSprite.Update(gameTime);
            if (_wallJoinSprite != null) _wallJoinSprite.Update(gameTime);
            if (_floorSprite != null) _floorSprite.Update(gameTime);

            _entities.ForEach(x => x.Update(gameTime));
        }

        public void DrawWalls(SpriteBatch spriteBatch, Vector2 position)
        {
            if (_leftWallSprite != null) _leftWallSprite.Draw(spriteBatch, position, Light);
            if (_rightWallSprite != null) _rightWallSprite.Draw(spriteBatch, position, Light);
            if (_wallJoinSprite != null) _wallJoinSprite.Draw(spriteBatch, position, Light);
        }

        public void DrawFloor(SpriteBatch spriteBatch, Vector2 position)
        {
            if (_floorSprite != null) _floorSprite.Draw(spriteBatch, position, Light);
        }

        public void DrawEntities(SpriteBatch spriteBatch, Vector2 position)
        {
            foreach (var entity in _entities)
            {
                entity.Draw(spriteBatch, position, Light);
            }
        }

        public void AddEntity(Entity entity)
        {
            var moveableEntity = entity as MoveableEntity;
            if (moveableEntity != null)
            {
                MoveableEntity = moveableEntity;
            }

            entity.Parent = this;
            _entities.Add(entity);

            _entities = new List<Entity>(_entities.OrderBy(x => x.ZIndex));
        }

        public void RemoveEntity(Entity entity)
        {
            _entities.Remove(entity);
        }
    }
}
