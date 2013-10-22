using System.Collections.Generic;
using System.Linq;
using Engine;
using Engine.Entities;
using Engine.Maps;
using Engine.Sprites;
using Microsoft.Xna.Framework;

namespace ZombieUnknown.Entities
{
    public class Human : MoveableEntity
    {
        private readonly Map _map;

        private List<Coordinate> _path;
        private Coordinate? _targetSquare;
        private bool _updateMapLocationAfterMove;

        public Vector2 MapPosition { get; set; }

        public Human(string name, Sprite sprite, Coordinate coordinate, Map map)
            : base(name, sprite, coordinate)
        {
            _map = map;

            MapPosition = Coordinate.ToVector2();
        }

        public void WalkPath(List<Coordinate> path)
        {
            _path = path;
        }
        
        public override void Update(GameTime gameTime)
        {
            if (_path != null)
            {
                if (!_targetSquare.HasValue)
                {
                    _targetSquare = _path.ElementAt(0);

                    if (_targetSquare.Value.X < Coordinate.X || _targetSquare.Value.Y > Coordinate.Y)
                    {
                        _updateMapLocationAfterMove = true;
                    }
                    else
                    {
                        _updateMapLocationAfterMove = false;

                        _map.RemoveEntity(Coordinate, this);

                        Coordinate = _path.ElementAt(0);

                        _map.AddEntity(Coordinate, this);
                    }
                }
                
                
                var distanceToTarget = (_targetSquare.Value.ToVector2() - MapPosition);
                var moveAmount = distanceToTarget;
                moveAmount.Normalize();
                moveAmount = moveAmount * (float)gameTime.ElapsedGameTime.TotalSeconds * 0.5f;

                if (moveAmount.Length() < distanceToTarget.Length())
                {
                    MapPosition += moveAmount;
                }
                else
                {
                    if (_updateMapLocationAfterMove)
                    {
                        _map.RemoveEntity(Coordinate, this);

                        Coordinate = _targetSquare.Value;
                        MapPosition = Coordinate.ToVector2();

                        _map.AddEntity(_targetSquare.Value, this);
                    }

                    _targetSquare = null;

                    _path.RemoveAt(0);
                    if (!_path.Any())
                    {
                        _path = null;
                    }
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(Color light)
        {
            SpriteDrawer.Draw(Sprite, MapPosition, light);
        }
    }
}
