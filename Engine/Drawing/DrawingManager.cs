using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Drawing
{
    public class DrawingManager
    {
        private readonly ICamera _camera;
        private readonly List<IDrawingProvider> _drawingProviders;
        private int _sortDepth;

        public DrawingManager(ICamera camera)
        {
            _camera = camera;
            _drawingProviders = new List<IDrawingProvider>();
        }

        public void RegisterProvider(IDrawingProvider drawingProvider)
        {
            _drawingProviders.Add(drawingProvider);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var drawingRequests = new List<DrawingRequest>();

            GatherDrawingRequests(drawingRequests);

            BuildTopologicalGraphSort(drawingRequests);

            Sort(drawingRequests);

            drawingRequests.Sort();

            foreach (var drawingRequest in drawingRequests)
            {
                drawingRequest.Draw(spriteBatch);
            }
        }

        private static void BuildTopologicalGraphSort(IEnumerable<DrawingRequest> drawingRequestsEnumerable)
        {
            var array = drawingRequestsEnumerable as DrawingRequest[] ?? drawingRequestsEnumerable.ToArray();

            foreach (var currentDrawingRequest in array)
            {
                foreach (var otherDrawingRequest in array)
                {
                    if (currentDrawingRequest == otherDrawingRequest)
                    {
                        continue;
                    }
                    
                    var currentAABB = currentDrawingRequest.WorldBoundingBox;
                    var otherAABB = otherDrawingRequest.WorldBoundingBox;

                    if ((otherAABB.Min.X < currentAABB.Max.X && otherAABB.Min.Y < currentAABB.Max.Y && otherAABB.Min.Z < currentAABB.Max.Z))
                    {
                        currentDrawingRequest.SpritesBehind.Add(otherDrawingRequest);
                    }
                }
            }
        }

        private void Sort(IEnumerable<DrawingRequest> drawingRequests)
        {
            _sortDepth = 0;
            foreach (var drawingRequest in drawingRequests)
            {
                VisitNode(drawingRequest);
            }
        }

        private void VisitNode(DrawingRequest node)
        {
            if (node.HasBeenVisited)
            {
                return;
            }

            node.HasBeenVisited = true;

            foreach(var spriteBehind in node.SpritesBehind)
            {
                VisitNode(spriteBehind);
            }

            node.IsoDepth = _sortDepth++;
        }

        private void GatherDrawingRequests(ICollection<DrawingRequest> drawingRequests)
        {
            foreach (var drawingProvider in _drawingProviders)
            {
                var providerDrawings = drawingProvider.GetDrawings();

                foreach (var providerDrawing in providerDrawings)
                {
                    Vector2 screenCoordinates;
                    if (_camera.GetScreenCoordinates(providerDrawing.MapPosition, out screenCoordinates))
                    {
                        providerDrawing.ScreenCoordinates = screenCoordinates;
                        drawingRequests.Add(providerDrawing);
                    }
                }
            }
        }
    }
}
