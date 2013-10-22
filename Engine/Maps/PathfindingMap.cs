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
            for (var x = 0; x < map.Width; x++)
            {
                for (var y = 0; y < map.Height; y++)
                {
                    var node = new Node(new Coordinate(x, y));
                    _nodes[x, y] = node;
                }
            }

            for (var x = 0; x < map.Width; x++)
            {
                for (var y = 0; y < map.Height; y++)
                {
                    var node = _nodes[x, y];

                    if (map.IsPositionOnMap(new Coordinate(x - 1, y)))
                    {
                        if (!map.GetTile(x, y).HasLeftWall)
                        {
                            node.AddNeighbor(_nodes[x - 1, y]);
                        }
                    }

                    if (map.IsPositionOnMap(new Coordinate(x, y - 1)))
                    {
                        var neighborNode = _nodes[x, y - 1];
                        if (!map.GetTile(x, y - 1).HasRightWall)
                        {
                            node.AddNeighbor(neighborNode);
                        }
                    }

                    if (map.IsPositionOnMap(new Coordinate(x + 1, y)))
                    {
                        var neighborNode = _nodes[x + 1, y];
                        if (!map.GetTile(x + 1, y).HasLeftWall)
                        {
                            node.AddNeighbor(neighborNode);
                        }
                    }

                    if (map.IsPositionOnMap(new Coordinate(x, y + 1)))
                    {
                        if (!map.GetTile(x, y).HasRightWall)
                        {
                            node.AddNeighbor(_nodes[x, y + 1]);
                        }
                    }
                }
            }
        }
    }
}
