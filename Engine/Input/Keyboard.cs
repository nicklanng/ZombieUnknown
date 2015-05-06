using System;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace Engine.Input
{
    public class Keyboard
    {
        public bool[] KeyDownArray { get; set; }

        public event EventHandler KeyPressed;
        public event EventHandler KeyReleased;

        private Keyboard() { }
        public static readonly Keyboard Instance = new Keyboard();

        public void Initialize()
        {
            KeyDownArray = new bool[255];
        }

        public void Update()
        {
            var keyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();

            var allKeys = Enum.GetValues(typeof (Keys));
            var pressedKeys = keyboardState.GetPressedKeys().ToList();

            foreach (Keys key in allKeys)
            {
                var wasPressed = KeyDownArray[(int)key];
                var nowPressed = pressedKeys.Contains(key);

                if (wasPressed != nowPressed)
                {
                    if (nowPressed)
                    {
                        KeyDownArray[(int)key] = true;
                        // fire pushed event
                        if (KeyPressed != null)
                        {
                            KeyPressed(this, new KeyEventArgs(key));
                        }
                    }
                    else
                    {
                        KeyDownArray[(int)key] = false;

                        if (KeyReleased != null)
                        {
                            KeyReleased(this, new KeyEventArgs(key));
                        }
                    }
                }
            }
        }

        public bool IsKeyDown(Keys key)
        {
            return KeyDownArray[(int) key];
        }
    }
}
