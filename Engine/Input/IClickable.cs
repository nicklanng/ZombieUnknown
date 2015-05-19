using System;
using Microsoft.Xna.Framework;

namespace Engine.Input
{
    public interface IClickable
    {
        Rectangle Bounds { get; }
        bool IsEnabled { get; }
        void Click();

        event EventHandler OnClick;
        bool TestClick(Vector2 clickPosition);
    }
}
