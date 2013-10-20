using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Engine.Pathfinding
{
    public class AStarSolver
    {
        private readonly Node _endingNode;
        private readonly List<SearchedNode> _closedList;
        private readonly List<SearchedNode> _openList;

        public List<Vector2> Solution { get; private set; } 

        public AStarSolver(Node startingNode, Node endingNode)
        {
            _endingNode = endingNode;

            _closedList = new List<SearchedNode>();
            _openList = new List<SearchedNode>();

            var startingSearchNode = new SearchedNode(startingNode, null, 0, CalculateManhattenHeuristic(startingNode));
            _openList.Add(startingSearchNode);
        }

        public void Solve()
        {
            var finished = false;
            while (!finished)
            {
                var lowestCostNode = _openList.OrderBy(x => x.Cost).First();

                finished = InvestigateNode(lowestCostNode);
            }
        }

        private bool InvestigateNode(SearchedNode currentNode)
        {
            _openList.Remove(currentNode);
            _closedList.Add(currentNode);

            foreach (var neighbor in currentNode.Neighbors)
            {
                if (neighbor == _endingNode)
                {
                    BuildSolutionPath(currentNode);
                    return true;
                }

                AddOrUpdateNeighbor(neighbor, currentNode, currentNode.Cost + 10, CalculateManhattenHeuristic(neighbor));
            }

            return false;
        }

        private void BuildSolutionPath(SearchedNode currentNode)
        {
            var list = new List<Vector2>();
            currentNode.GetPath(list).Add(_endingNode.Position);
            list.RemoveAt(0);
            Solution = list;
        }

        private void AddOrUpdateNeighbor(Node neighbor, SearchedNode currentNode, int travelledCost, int heuristic)
        {
            var savedNode = _openList.SingleOrDefault(x => x.Position == neighbor.Position);

            if (savedNode == null)
            {
                _openList.Add(new SearchedNode(neighbor, currentNode, travelledCost, heuristic));
            }
            else
            {
                if (savedNode.Cost > travelledCost + heuristic)
                {
                    savedNode.Reparent(currentNode, travelledCost, heuristic);
                }
            }
        }

        private int CalculateManhattenHeuristic(Node currentNode)
        {
            var x = Math.Abs(_endingNode.Position.X - currentNode.Position.X);
            var y = Math.Abs(_endingNode.Position.Y - currentNode.Position.Y);

            return (int)(x + y);
        }
    }
}
