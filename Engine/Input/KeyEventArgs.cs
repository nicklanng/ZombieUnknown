using System;
using Microsoft.Xna.Framework.Input;

namespace Engine.Input
{
    public class KeyEventArgs : EventArgs 
    {
        public Keys Key { get; private set; }

        public KeyEventArgs(Keys key)
        {
            Key = key;
        }
    }
}
