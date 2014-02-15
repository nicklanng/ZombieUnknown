using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Drawing
{
    public class VirtualScreen
    {
        public readonly int VirtualWidth;
        public readonly int VirtualHeight;
        public readonly float VirtualAspectRatio;

        private readonly GraphicsDevice _graphicsDevice;
        private readonly RenderTarget2D _screen;

        private Rectangle _area;
        private bool _areaIsDirty = true;

        public VirtualScreen(int virtualWidth, int virtualHeight, GraphicsDevice graphicsDevice)
        {
            VirtualWidth = virtualWidth;
            VirtualHeight = virtualHeight;
            VirtualAspectRatio = virtualWidth / (float)(virtualHeight);

            _graphicsDevice = graphicsDevice;
            _screen = new RenderTarget2D(graphicsDevice, virtualWidth, virtualHeight, false, graphicsDevice.PresentationParameters.BackBufferFormat, graphicsDevice.PresentationParameters.DepthStencilFormat, graphicsDevice.PresentationParameters.MultiSampleCount, RenderTargetUsage.DiscardContents);
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
            var physicalWidth = _graphicsDevice.Viewport.Width;
            var physicalHeight = _graphicsDevice.Viewport.Height;
            var physicalAspectRatio = _graphicsDevice.Viewport.AspectRatio;

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
            _graphicsDevice.SetRenderTarget(_screen);
        }

        public void EndCapture()
        {
            _graphicsDevice.SetRenderTarget(null);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_screen, _area, Color.White);
        }
    }
}
