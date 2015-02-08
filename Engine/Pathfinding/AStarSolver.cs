using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Maps;

namespace Engine.Pathfinding
{
    public class AStarSolver
    {
        private readonly Node _startingNode;
        private readonly Node _endingNode;

        public List<Coordinate> Solution { get; private set; } 

        public AStarSolver(Node startingNode, Node endingNode)
        {
            _startingNode = startingNode;
            _endingNode = endingNode;
        }

        public bool Solve()
        {
            var path = FindPath(_startingNode, _endingNode);

            if (path == null) return false;

            Solution = path.ToCoordinateList();
            Solution.Reverse();

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
                foreach (var neighbor in path.LastStep.Neighbors)
                {
                    var d = path.TotalCost + GetStepCost(path.LastStep, neighbor);
                    var newPath = path.AddStep(neighbor, d);
                    queue.Enqueue(newPath.TotalCost + CalculateManhattenHeuristic(neighbor), newPath);
                }
            }
            return null;
        }

        private int CalculateManhattenHeuristic(Node currentNode)
        {
            var x = Math.Abs(_endingNode.Coordinate.X - currentNode.Coordinate.X);
            var y = Math.Abs(_endingNode.Coordinate.Y - currentNode.Coordinate.Y);

            return x + y;
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
