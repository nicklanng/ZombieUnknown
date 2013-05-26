using System.Collections.Generic;
using Engine.Entities;
using Engine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Isometric
{
    public class Tile
    {
        private Color _light;
        private readonly List<Entity> _entities;

        public Vector2 Position { get; private set; }

        public Sprite FloorSprite { get; private set; }
        public Sprite LeftWallSprite { get; private set; }
        public Sprite RightWallSprite { get; private set; }
        public Sprite WallJoinSprite { get; private set; }

        public MoveableEntity MoveableEntity { get; private set; }

        public Tile(Vector2 position, Sprite floorSprite, Sprite leftWall, Sprite rightWall, Sprite wallJoinSprite)
        {
            Position = position;
            FloorSprite = floorSprite;
            LeftWallSprite = leftWall;
            RightWallSprite = rightWall;
            WallJoinSprite = wallJoinSprite;

            _entities = new List<Entity>();
            _light = Color.White;
        }

        public void SetLight(Color light)
        {
            _light = light;
        }

        public void Update(GameTime gameTime)
        {
            if (LeftWallSprite != null) LeftWallSprite.Update(gameTime);
            if (RightWallSprite != null) RightWallSprite.Update(gameTime);
            if (WallJoinSprite != null) WallJoinSprite.Update(gameTime);
            if (FloorSprite != null) FloorSprite.Update(gameTime);

            _entities.ForEach(x => x.Update(gameTime));
        }

        public void DrawWalls(SpriteBatch spriteBatch, Vector2 position)
        {
            if (LeftWallSprite != null) LeftWallSprite.Draw(spriteBatch, position, _light);
            if (RightWallSprite != null) RightWallSprite.Draw(spriteBatch, position, _light);
            if (WallJoinSprite != null) WallJoinSprite.Draw(spriteBatch, position, _light);
        }

        public void DrawFloor(SpriteBatch spriteBatch, Vector2 position)
        {
            if (FloorSprite != null) FloorSprite.Draw(spriteBatch, position, _light);
        }

        public void DrawEntities(SpriteBatch spriteBatch, Vector2 position)
        {
            foreach (var entity in _entities)
            {
                entity.Draw(spriteBatch, position, _light);
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
        }
    }
}
