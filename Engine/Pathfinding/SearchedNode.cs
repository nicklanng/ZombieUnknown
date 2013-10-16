using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Engine.Pathfinding
{
    class SearchedNode
    {
        private readonly Node _node;

        private SearchedNode _parent;
        private int _travelledCost;
        private int _heuristic;

        public int Cost
        {
            get { return _travelledCost + _heuristic; }
        }

        public IEnumerable<Node> Neighbors
        {
            get { return _node.Neighbors; }
        }

        public Vector2 Position
        {
            get { return _node.Position; }
        }

        public List<Vector2> GetPath(List<Vector2> list)
        {
            if (_parent != null)
            {
                _parent.GetPath(list);
            }

            list.Add(_node.Position);

            return list;
        }

        public SearchedNode(Node node, SearchedNode parent, int travelledCost, int heuristic)
        {
            _node = node;
            _parent = parent;
            _travelledCost = travelledCost;
            _heuristic = heuristic;
        }

        public void Reparent(SearchedNode parent, int travelledCost, int heuristic)
        {
            _parent = parent;
            _travelledCost = travelledCost;
            _heuristic = heuristic;
        }
    }
}
