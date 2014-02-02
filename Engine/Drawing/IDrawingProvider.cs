using System.Collections.Generic;

namespace Engine.Drawing
{
    public interface IDrawingProvider
    {
        IEnumerable<DrawingRequest> GetDrawings();
    }
}
