using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Engine.Pathfinding
{
    class Node
    {
        private readonly Vector2 _position;
        private readonly List<Node> _neighbors;

        public Node(Vector2 position)
        {
            _position = position;
            _neighbors = new List<Node>();
        }

        public void AddNeighbor(Node neighbor)
        {
            _neighbors.Add(neighbor);
        }

        public override string ToString()
        {
            var value = _position + Environment.NewLine;
            return _neighbors.Aggregate(value, (current, neighbor) => current + ("\t" + neighbor._position));
        }
    }
}
