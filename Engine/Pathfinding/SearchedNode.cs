using System.Collections.Generic;
using Engine.Maps;

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

        public Coordinate Position
        {
            get { return _node.Coordinate; }
        }

        public List<Coordinate> GetPath(List<Coordinate> list)
        {
            if (_parent != null)
            {
                _parent.GetPath(list);
            }

            list.Add(_node.Coordinate);

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
