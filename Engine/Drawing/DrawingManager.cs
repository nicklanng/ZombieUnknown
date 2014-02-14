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

                DrawBorder(spriteBatch, drawingRequest.WorldBoundingBox, 1, Color.Red);
            }
        }

        private void DrawBorder(SpriteBatch spriteBatch, BoundingBox rectangleToDraw, int thicknessOfBorder, Color borderColor)
        {
            var pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White });

            // Draw top line
            spriteBatch.Draw(pixel, new Rectangle((int)rectangleToDraw.Min.X, (int)rectangleToDraw.Min.Y, (int)(rectangleToDraw.Max.X - rectangleToDraw.Min.X), thicknessOfBorder), borderColor);

            // Draw left line
            spriteBatch.Draw(pixel, new Rectangle((int)rectangleToDraw.Min.X, (int)rectangleToDraw.Min.Y, thicknessOfBorder, (int)(rectangleToDraw.Max.Y - rectangleToDraw.Min.Y)), borderColor);

            // Draw right line
            spriteBatch.Draw(pixel, new Rectangle(((int)rectangleToDraw.Max.X - thicknessOfBorder),
                                            (int)rectangleToDraw.Min.Y,
                                            thicknessOfBorder,
                                            (int)(rectangleToDraw.Max.Y - rectangleToDraw.Min.Y)), borderColor);
            // Draw bottom line
            spriteBatch.Draw(pixel, new Rectangle((int)rectangleToDraw.Min.X,
                                            (int)rectangleToDraw.Max.Y - thicknessOfBorder,
                                            (int)(rectangleToDraw.Max.X - rectangleToDraw.Min.X),
                                            thicknessOfBorder), borderColor);
        }

        private static void BuildTopologicalGraphSort(IEnumerable<DrawingRequest> drawingRequestsEnumerable)
        {
            var array = drawingRequestsEnumerable as DrawingRequest[] ?? drawingRequestsEnumerable.ToArray();

            foreach (var currentDrawingRequest in array)
            {
                foreach (var comparitorDrawingRequest in array)
                {
                    if (currentDrawingRequest == comparitorDrawingRequest)
                    {
                        continue;
                    }
                    
                    var currentAABB = currentDrawingRequest.WorldBoundingBox;
                    var otherAABB = comparitorDrawingRequest.WorldBoundingBox;

                    if (otherAABB.Max.X < currentAABB.Min.X ||
                        otherAABB.Max.Y < currentAABB.Min.Y ||
                        otherAABB.Max.Z < currentAABB.Min.Z)
                    {
                        currentDrawingRequest.SpritesBehind.Add(comparitorDrawingRequest);
                    }

                    if (otherAABB == currentAABB && comparitorDrawingRequest.DrawingLevel < currentDrawingRequest.DrawingLevel)
                    {
                        currentDrawingRequest.SpritesBehind.Add(comparitorDrawingRequest);
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
                        providerDrawing.SetScreenCoordinates(screenCoordinates);
                        drawingRequests.Add(providerDrawing);
                    }
                }
            }
        }
    }
}
