using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Drawing
{
    public class UIManager
    {
        private readonly List<IUIProvider> _uiProviders;
        
        public UIManager()
        {
            _uiProviders = new List<IUIProvider>();
        }

        public void RegisterProvider(IUIProvider drawingProvider)
        {
            _uiProviders.Add(drawingProvider);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var uiRequests = new List<UIRequest>();

            GatherUIRequests(uiRequests);

            foreach (var uiRequest in uiRequests)
            {
                uiRequest.Draw(spriteBatch);
            }
        }

        private void GatherUIRequests(ICollection<UIRequest> uiRequests)
        {
            var providerDrawings = _uiProviders.SelectMany(x => x.GetDrawings());
           
            foreach (var providerDrawing in providerDrawings)
            {
                uiRequests.Add(providerDrawing);
            }
        }
    }
}
