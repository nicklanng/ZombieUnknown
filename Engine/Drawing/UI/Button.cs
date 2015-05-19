using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Input;
using Engine.Sprites;
using Microsoft.Xna.Framework;

namespace Engine.Drawing.UI
{
    public class Button : IUIProvider, IClickable
    {
        private readonly Sprite _backgroundSprite;
        private readonly Font _font;
        private readonly string _text;
        private readonly int _padding;
        private readonly Vector2 _actualPosition;
        private readonly int _textWidth;

        public event EventHandler OnClick;

        public Button(Sprite backgroundSprite, Font font, string text, UIPosition position, int padding)
        {
            _backgroundSprite = backgroundSprite;
            _font = font;
            _text = text;
            _padding = padding;

            var position1 = position;
            var maxWidth = GameState.VirtualScreen.VirtualWidth;
            var maxHeight = GameState.VirtualScreen.VirtualHeight;
            switch (position.Achor)
            {
                case UIAnchor.BottomLeft:
                    _actualPosition.X = position1.Offset.X;
                    _actualPosition.Y = maxHeight - position1.Offset.Y - _backgroundSprite.Height;
                    break;
                case UIAnchor.BottomRight:
                    _actualPosition.X = maxWidth - position1.Offset.X - _backgroundSprite.Width;
                    _actualPosition.Y = maxHeight - position1.Offset.Y - _backgroundSprite.Height;
                    break;
                case UIAnchor.TopLeft:
                    _actualPosition.X = position1.Offset.X;
                    _actualPosition.Y = position1.Offset.Y;
                    break;
                case UIAnchor.TopRight:
                    _actualPosition.X = maxWidth - position1.Offset.X - _backgroundSprite.Width;
                    _actualPosition.Y = position1.Offset.Y;
                    break;
            }

            var characterSprites = _text.Select(x => _font.GetSprite(x)).ToArray();
            _textWidth = characterSprites.Sum(x => x.Width) + (characterSprites.Count() - 1) * 2;

            ClickLocationManager.Instance.RegisterClickLocation(this);
        }

        public IEnumerable<UIRequest> GetDrawings()
        {
            yield return new UIRequest(_backgroundSprite, _actualPosition, 0.6f);

            var characterX = _actualPosition.X + (_backgroundSprite.Width/2f) - (_textWidth / 2f);
            var characterY = _actualPosition.Y + _padding;

            foreach (var character in _text)
            { 
                yield return new UIRequest(_font.GetSprite(character), new Vector2(characterX, characterY), 0.61f);
                characterX = characterX + _font.Width + 2;
            }
        }

        public Rectangle Bounds
        {
            get
            {
                var screenPosition = GameState.VirtualScreen.ConvertVirtualScreenCoordinatesToScreenCoordinates(_actualPosition);
                var screenSize = GameState.VirtualScreen.ConvertVirtualScreenCoordinatesToScreenCoordinates(new Vector2(_backgroundSprite.Width, _backgroundSprite.Height));
                return new Rectangle((int)screenPosition.X, (int)screenPosition.Y, (int)screenSize.X, (int)screenSize.Y);
            }
        }

        public bool IsEnabled
        {
            get { return true; }
        }

        public void Click()
        {
            if (OnClick != null)
            {
                OnClick(this, new EventArgs());
            }
        }
    }
}
