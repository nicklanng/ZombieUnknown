using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Engine.Pathfinding
{
    public class Node
    {
        public Vector2 Position { get; private set; }
        public List<Node> Neighbors { get; set; }

        public Node(Vector2 position)
        {
            Position = position;
            Neighbors = new List<Node>();
        }

        public void AddNeighbor(Node neighbor)
        {
            Neighbors.Add(neighbor);
        }

        public override string ToString()
        {
            var value = Position + Environment.NewLine;
            return Neighbors.Aggregate(value, (current, neighbor) => current + ("\t" + neighbor.Position));
        }
    }
}
