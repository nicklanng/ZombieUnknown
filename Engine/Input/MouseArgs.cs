using System;
using Microsoft.Xna.Framework;

namespace Engine.Input
{
    public class MouseArgs : EventArgs 
    {
        public Vector2 ScreenPosition { get; set; }

        public MouseArgs(Vector2 screenPosition)
        {
            ScreenPosition = screenPosition;
        }
    }
}
