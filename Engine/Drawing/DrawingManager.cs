using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Drawing
{
    public class DrawingManager
    {
        private readonly ICamera _camera;
        private readonly List<IDrawingProvider> _drawingProviders;

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

            drawingRequests.Sort();

            foreach (var drawingRequest in drawingRequests)
            {
                drawingRequest.Draw(spriteBatch);
            }
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
