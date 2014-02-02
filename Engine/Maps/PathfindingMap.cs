using Engine.Pathfinding;

namespace Engine.Maps
{
    public class PathfindingMap
    {
        private Node[,] _nodes;

        public PathfindingMap(Map map)
        {
            RegeneratePathfindingMap(map);
        }

        public Node GetNodeAt(Coordinate coordinate)
        {
            return _nodes[coordinate.X, coordinate.Y];
        }

        private void RegeneratePathfindingMap(Map map)
        {
            _nodes = new Node[map.Width, map.Height];

            for (var y = 0; y < map.Height; y++)
            {
                for (var x = 0; x < map.Width; x++)
                {
                    var node = new Node(new Coordinate(x, y));
                    _nodes[x, y] = node;
                }
            }

            for (var y = 0; y < map.Height; y++)
            {
                for (var x = 0; x < map.Width; x++)
                {
                    var node = _nodes[x, y];

                    var thisCoord = new Coordinate(x, y);
                    var thisTile = map.GetTile(thisCoord);

                    var upCoord = new Coordinate(x - 1, y - 1);
                    var upTile = map.GetTile(upCoord);

                    var upLeftCoord = new Coordinate(x, y - 1);
                    var upLeftTile = map.GetTile(upLeftCoord);

                    var leftCoord = new Coordinate(x + 1, y - 1);
                    var leftTile = map.GetTile(leftCoord);

                    var downLeftCoord = new Coordinate(x + 1, y);
                    var downLeftTile = map.GetTile(downLeftCoord);

                    var downCoord = new Coordinate(x + 1, y + 1);
                    var downTile = map.GetTile(downCoord);

                    var downRightCoord = new Coordinate(x, y + 1);
                    var downRightTile = map.GetTile(downRightCoord);

                    var rightCoord = new Coordinate(x - 1, y + 1);
                    var rightTile = map.GetTile(rightCoord);

                    var upRightCoord = new Coordinate(x - 1, y);
                    var upRightTile = map.GetTile(upRightCoord);

                    // up
                    if (map.IsPositionOnMap(upCoord))
                    {
                        if (!upTile.HasLeftWall && !upTile.HasRightWall && !upLeftTile.HasRightWall && !upRightTile.HasLeftWall)
                        {
                            node.AddNeighbor(_nodes[upCoord.X, upCoord.Y]);
                        }
                    }

                    // up left
                    if (map.IsPositionOnMap(upLeftCoord))
                    {
                        if (!upLeftTile.HasRightWall)
                        {
                            node.AddNeighbor(_nodes[upLeftCoord.X, upLeftCoord.Y]);
                        }
                    }

                    // left
                    if (map.IsPositionOnMap(leftCoord))
                    {
                        if (leftTile == null || upLeftTile == null)
                        {
                            continue;
                        }

                        if (!thisTile.HasLeftWall && !leftTile.HasRightWall && !upLeftTile.HasLeftWall && !upLeftTile.HasRightWall)
                        {
                            node.AddNeighbor(_nodes[leftCoord.X, leftCoord.Y]);
                        }
                    }

                    // down left
                    if (map.IsPositionOnMap(downLeftCoord))
                    {
                        if (!thisTile.HasLeftWall)
                        {
                            node.AddNeighbor(_nodes[downLeftCoord.X, downLeftCoord.Y]);
                        }
                    }

                    // down
                    if (map.IsPositionOnMap(downCoord))
                    {
                        if (downLeftTile == null || downRightTile == null)
                        {
                            continue;
                        }

                        if (!thisTile.HasLeftWall && !thisTile.HasRightWall && !downLeftTile.HasRightWall && !downRightTile.HasLeftWall)
                        {
                            node.AddNeighbor(_nodes[downCoord.X, downCoord.Y]);
                        }
                    }

                    // down right
                    if (map.IsPositionOnMap(downRightCoord))
                    {
                        if (!downRightTile.HasRightWall)
                        {
                            node.AddNeighbor(_nodes[downRightCoord.X, downRightCoord.Y]);
                        }
                    }

                    // right
                    if (map.IsPositionOnMap(rightCoord))
                    {
                        if (rightTile == null || upRightTile == null)
                        {
                            continue;
                        }

                        if (!thisTile.HasRightWall && !rightTile.HasLeftWall && !upRightTile.HasLeftWall && !upRightTile.HasRightWall)
                        {
                            node.AddNeighbor(_nodes[rightCoord.X, rightCoord.Y]);
                        }
                    }

                    // up right
                    if (map.IsPositionOnMap(upRightCoord))
                    {
                        if (!upRightTile.HasLeftWall)
                        {
                            node.AddNeighbor(_nodes[upRightCoord.X, upRightCoord.Y]);
                        }
                    }
                }
            }
        }
    }
}
