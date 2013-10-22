using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Maps;

namespace Engine.Pathfinding
{
    public class Node
    {
        public Coordinate Coordinate { get; private set; }
        public List<Node> Neighbors { get; set; }

        public Node(Coordinate coordinate)
        {
            Coordinate = coordinate;
            Neighbors = new List<Node>();
        }

        public void AddNeighbor(Node neighbor)
        {
            Neighbors.Add(neighbor);
        }

        public override string ToString()
        {
            var value = Coordinate + Environment.NewLine;
            return Neighbors.Aggregate(value, (current, neighbor) => current + ("\t" + neighbor.Coordinate));
        }
    }
}
