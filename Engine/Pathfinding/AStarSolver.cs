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
        private readonly HashSet<SearchedNode> _closedList;
        private readonly PriorityQueue<int, SearchedNode> _openList;

        public List<Coordinate> Solution { get; private set; } 

        public AStarSolver(Node startingNode, Node endingNode)
        {
            _startingNode = startingNode;
            _endingNode = endingNode;
        }

        public void Solve()
        {
            var path = FindPath(_startingNode, _endingNode);
            Solution = path.ToCoordinateList();
            Solution.Reverse();
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
                    var d = path.TotalCost + 10; // diagnoals?
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
    }
}
