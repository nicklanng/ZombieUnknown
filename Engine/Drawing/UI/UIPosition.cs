using Microsoft.Xna.Framework;

namespace Engine.Drawing.UI
{
    public class UIPosition
    {
        public Vector2 Offset { get; private set; }
        public UIAnchor Achor { get; private set; }

        public UIPosition(Vector2 offset, UIAnchor achor)
        {
            Offset = offset;
            Achor = achor;
        }
    }
}
