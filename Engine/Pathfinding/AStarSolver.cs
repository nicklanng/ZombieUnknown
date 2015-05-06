using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Maps;
using Microsoft.Xna.Framework;

namespace Engine.Pathfinding
{
    public class AStarSolver
    {
        private readonly int _maxPathLength;
        private readonly Node _startingNode;
        private readonly Node _endingNode;

        public List<Vector2> Solution { get; private set; } 

        public AStarSolver(Node startingNode, Node endingNode)
        {
            _startingNode = startingNode;
            _endingNode = endingNode;
        }

        public AStarSolver(Node startingNode, Node endingNode, int maxPathLength) 
            : this(startingNode, endingNode)
        {
            _maxPathLength = maxPathLength;
        }

        public bool Solve()
        {
            var path = FindPath(_startingNode, _endingNode);

            if (path == null) return false;

            Solution = path.ToWaypoints();

            return true;
        }

        private Path FindPath(Node start, Node destination)
        {
            var closed = new HashSet<Node>();
            var queue = new PriorityQueue<double, Path>();
            queue.Enqueue(0, new Path(start));
            while (!queue.IsEmpty)
            {
                var path = queue.Dequeue();
                if (closed.Contains(path.LastStep))
                    continue;
                if (path.LastStep.Equals(destination))
                    return path;
                closed.Add(path.LastStep);
                if (_maxPathLength > 1 && path.Count() == _maxPathLength)
                {
                    return path;
                }
                foreach (var neighbor in path.LastStep.Neighbors)
                {
                    var d = path.TotalCost + GetStepCost(path.LastStep, neighbor);
                    var newPath = path.AddStep(neighbor, d);
                    queue.Enqueue(newPath.TotalCost + CalculateHeuristic(neighbor), newPath);
                }
            }
            return null;
        }

        private int CalculateHeuristic(Node currentNode)
        {
            var x = Math.Abs(_endingNode.Coordinate.X - currentNode.Coordinate.X);
            var y = Math.Abs(_endingNode.Coordinate.Y - currentNode.Coordinate.Y);
            
            return 10 * (x + y) - 4 * Math.Abs(x - y);
        }

        private int GetStepCost(Node lastStep, Node thisStep)
        {
            var movementCoordinate = thisStep.Coordinate - lastStep.Coordinate;
            var direction = Direction.CoordinateDirectionMap[movementCoordinate];

            if (direction == Direction.NorthEast || direction == Direction.NorthWest ||
                direction == Direction.SouthEast || direction == Direction.SouthWest)
            {
                return 14;
            }

            return 10;
        }
    }
}
