using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Drawing
{
    public class VirtualScreen
    {
        public readonly int VirtualWidth;
        public readonly int VirtualHeight;
        public readonly float VirtualAspectRatio;

        private RenderTarget2D _screen;

        private Rectangle _area;
        private bool _areaIsDirty = true;
        private int _physicalWidth;
        private int _physicalHeight;

        public VirtualScreen(int virtualWidth, int virtualHeight)
        {
            VirtualWidth = virtualWidth;
            VirtualHeight = virtualHeight;
            VirtualAspectRatio = virtualWidth / (float)(virtualHeight);

            ReInitialize();
        }

        private void ReInitialize()
        {
            _screen = new RenderTarget2D(GameState.GraphicsDevice, VirtualWidth, VirtualHeight, false, GameState.GraphicsDevice.PresentationParameters.BackBufferFormat, GameState.GraphicsDevice.PresentationParameters.DepthStencilFormat, GameState.GraphicsDevice.PresentationParameters.MultiSampleCount, RenderTargetUsage.DiscardContents);
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
            _physicalWidth = GameState.GraphicsDevice.Viewport.Width;
            _physicalHeight = GameState.GraphicsDevice.Viewport.Height;
            var physicalAspectRatio = GameState.GraphicsDevice.Viewport.AspectRatio;

            if ((int)(physicalAspectRatio * 10) == (int)(VirtualAspectRatio * 10))
            {
                _area = new Rectangle(0, 0, _physicalWidth, _physicalHeight);
                return;
            }
            
            if (VirtualAspectRatio > physicalAspectRatio)
            {
                var scaling = _physicalWidth / (float)VirtualWidth;
                var width = VirtualWidth * scaling;
                var height = VirtualHeight * scaling;
                var borderSize = (int)((_physicalHeight - height) / 2);
                _area = new Rectangle(0, borderSize, (int)width, (int)height);
            }
            else
            {
                var scaling = _physicalHeight / (float)VirtualHeight;
                var width = VirtualWidth * scaling;
                var height = VirtualHeight * scaling;
                var borderSize = (int)((_physicalWidth - width) / 2);
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
            var widthScalar =  VirtualWidth * 1.0f / _physicalWidth * 1.0f;
            var heightScalar =  VirtualHeight * 1.0f / _physicalHeight * 1.0f;

            return new Vector2(coordinates.X * widthScalar, coordinates.Y * heightScalar);
        }

        public Vector2 ConvertVirtualScreenCoordinatesToScreenCoordinates(Vector2 coordinates)
        {
            var widthScalar = _physicalWidth * 1.0f / VirtualWidth * 1.0f;
            var heightScalar = _physicalHeight * 1.0f / VirtualHeight * 1.0f;

            return new Vector2(coordinates.X * widthScalar, coordinates.Y * heightScalar);
        }
    }
}
