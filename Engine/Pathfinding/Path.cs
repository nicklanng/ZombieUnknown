using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Engine.Maps;
using Microsoft.Xna.Framework;

namespace Engine.Pathfinding
{
    public class Path : IEnumerable<Node>
    {
        public Node LastStep { get; private set; }
        public Path PreviousSteps { get; private set; }
        public double TotalCost { get; private set; }
        private Path(Node lastStep, Path previousSteps, double totalCost)
        {
            LastStep = lastStep;
            PreviousSteps = previousSteps;
            TotalCost = totalCost;
        }
        public Path(Node start) : this(start, null, 0) { }
        public Path AddStep(Node step, double stepCost)
        {
            return new Path(step, this, TotalCost + stepCost);
        }
        public IEnumerator<Node> GetEnumerator()
        {
            for (var p = this; p != null; p = p.PreviousSteps)
                yield return p.LastStep;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public List<Vector2> ToWaypoints()
        {
            var result = new List<Vector2> { LastStep.Coordinate };
            var lastDirection = Vector2.Zero;

            if (PreviousSteps != null)
            {
                foreach (var node in PreviousSteps)
                {
                    var coordinates = node.Coordinate;
                    var direction = (Vector2) coordinates - result.Last();
                    if (direction == lastDirection)
                    {
                        result.RemoveAt(result.Count - 1);
                    }
                    lastDirection = direction;
                    result.Add(coordinates);
                }

                result.Reverse();
            }

            return result;
        }
    }
}
