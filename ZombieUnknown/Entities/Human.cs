using System.Collections.Generic;
using System.Linq;
using Engine.Entities;
using Engine.Maps;
using Engine.Sprites;
using Microsoft.Xna.Framework;

namespace ZombieUnknown.Entities
{
    public class Human : MoveableEntity
    {
        private readonly Map _map;

        private List<Vector2> _path;
        private Vector2? _targetSquare;
        private bool updateMapLocationAfterMove;
        private Vector2 _oldPosition;

        public Human(string name, Sprite sprite, Vector2 mapPosition, Map map)
            : base(name, sprite, mapPosition)
        {
            _map = map;
        }

        public void WalkPath(List<Vector2> path)
        {
            _path = path;
        }
        
        public override void Update(GameTime gameTime)
        {
            //if (_path != null && !_targetSquare.HasValue && _path.Count > 0)
            //{
            //    MoveTo(_path.ElementAt(0));
            //}

            //if (_targetSquare.HasValue)
            //{
            //    var distanceToTarget = (_targetSquare.Value - MapPosition);
            //    var moveAmount = distanceToTarget;
            //    moveAmount.Normalize();
            //    moveAmount = moveAmount * (float)gameTime.ElapsedGameTime.TotalSeconds * 0.5f;

            //    if (moveAmount.Length() < distanceToTarget.Length())
            //    {
            //        MapPosition += moveAmount;
            //    }
            //    else
            //    {
            //        MapPosition = _targetSquare.Value;
            //        _targetSquare = null;
            //        _path.RemoveAt(0);
            //    }
            //}




            if (_path != null)
            {
                if (!_targetSquare.HasValue)
                {
                    _oldPosition = MapPosition;

                    var nextTargetSquare = _path.ElementAt(0);
                    if (nextTargetSquare.X < _oldPosition.X || nextTargetSquare.Y > _oldPosition.Y)
                    {
                        updateMapLocationAfterMove = true;
                    }
                    else
                    {
                        updateMapLocationAfterMove = false;
                        _map.RemoveEntity(_oldPosition, this);
                        _map.AddEntity(_path.ElementAt(0), this);
                    }

                    _targetSquare = _path.ElementAt(0);
                }
                
                
                var distanceToTarget = (_targetSquare.Value - MapPosition);
                var moveAmount = distanceToTarget;
                moveAmount.Normalize();
                moveAmount = moveAmount * (float)gameTime.ElapsedGameTime.TotalSeconds * 0.5f;

                if (moveAmount.Length() < distanceToTarget.Length())
                {
                    MapPosition += moveAmount;
                }
                else
                {
                    MapPosition = _targetSquare.Value;
                    _path.RemoveAt(0);
                    if (!_path.Any())
                    {
                        _path = null;
                    }

                    if (updateMapLocationAfterMove)
                    {
                        _map.RemoveEntity(_oldPosition, this);

                        _map.AddEntity(_targetSquare.Value, this);
                    }

                    _targetSquare = null;
                }
            }




            base.Update(gameTime);
        }
    }
}
