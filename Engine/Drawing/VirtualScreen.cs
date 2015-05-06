using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Drawing
{
    public class VirtualScreen
    {
        public readonly int VirtualWidth;
        public readonly int VirtualHeight;
        public readonly float VirtualAspectRatio;

        private readonly RenderTarget2D _screen;

        private Rectangle _area;
        private bool _areaIsDirty = true;

        public VirtualScreen(int virtualWidth, int virtualHeight)
        {
            VirtualWidth = virtualWidth;
            VirtualHeight = virtualHeight;
            VirtualAspectRatio = virtualWidth / (float)(virtualHeight);

            _screen = new RenderTarget2D(GameState.GraphicsDevice, virtualWidth, virtualHeight, false, GameState.GraphicsDevice.PresentationParameters.BackBufferFormat, GameState.GraphicsDevice.PresentationParameters.DepthStencilFormat, GameState.GraphicsDevice.PresentationParameters.MultiSampleCount, RenderTargetUsage.DiscardContents);
        }

        public void PhysicalResolutionChanged()
        {
            _areaIsDirty = true;
        }

        public void Update()
        {
            if (!_areaIsDirty)
            {
                return;
            }

            _areaIsDirty = false;
            var physicalWidth = GameState.GraphicsDevice.Viewport.Width;
            var physicalHeight = GameState.GraphicsDevice.Viewport.Height;
            var physicalAspectRatio = GameState.GraphicsDevice.Viewport.AspectRatio;

            if ((int)(physicalAspectRatio * 10) == (int)(VirtualAspectRatio * 10))
            {
                _area = new Rectangle(0, 0, physicalWidth, physicalHeight);
                return;
            }


            if (VirtualAspectRatio > physicalAspectRatio)
            {
                var scaling = physicalWidth / (float)VirtualWidth;
                var width = VirtualWidth * scaling;
                var height = VirtualHeight * scaling;
                var borderSize = (int)((physicalHeight - height) / 2);
                _area = new Rectangle(0, borderSize, (int)width, (int)height);
            }
            else
            {
                var scaling = physicalHeight / (float)VirtualHeight;
                var width = VirtualWidth * scaling;
                var height = VirtualHeight * scaling;
                var borderSize = (int)((physicalWidth - width) / 2);
                _area = new Rectangle(borderSize, 0, (int)width, (int)height);
            }
        }

        public void BeginCapture()
        {
            GameState.GraphicsDevice.SetRenderTarget(_screen);
        }

        public void EndCapture()
        {
            GameState.GraphicsDevice.SetRenderTarget(null);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_screen, _area, Color.White);
        }

        public Vector2 ConvertScreenCoordinatesToVirtualScreenCoordinates(Vector2 coordinates)
        {
            var widthScalar = VirtualWidth * 1.0f / GameState.GraphicsDevice.Viewport.Width * 1.0f;
            var heightScalar = VirtualHeight * 1.0f / GameState.GraphicsDevice.Viewport.Height * 1.0f;

            return new Vector2(coordinates.X * widthScalar, coordinates.Y * heightScalar);
        }
    }
}
