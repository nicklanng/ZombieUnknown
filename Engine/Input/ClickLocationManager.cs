using System.Collections.Generic;
using System.Linq;
using Engine.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Input
{
    public class ClickLocationManager
    {
        public bool IsEnabled { get; set; }

        private static ClickLocationManager _instance;
        private readonly List<IClickable> _clickLocations;

        public static ClickLocationManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ClickLocationManager();
                }
                return _instance;
            }
        }

        private ClickLocationManager()
        {
            _clickLocations = new List<IClickable>();
        }

        public void RegisterClickLocation(IClickable clickable)
        {
            _clickLocations.Add(clickable);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsEnabled) return;

            var whiteDot = new Texture2D(GameState.GraphicsDevice, 1, 1);
            whiteDot.SetData(new[] { Color.White });

            foreach (var clickLocation in _clickLocations.Where(clickLocation => clickLocation.IsEnabled))
            {
                PrimiviteDrawing.DrawRectangle(whiteDot, spriteBatch, clickLocation.Bounds, 1, Color.Red);
            }
        }

        public void TryToClick(Vector2 clickPosition)
        {
            foreach (var clickLocation in _clickLocations.Where(clickLocation => clickLocation.IsEnabled))
            {
                if (clickLocation.Bounds.Contains(clickPosition))
                {
                    clickLocation.Click();
                    return;
                }
            }
        }
    }
}
