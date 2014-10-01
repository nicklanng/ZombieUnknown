using System.Collections.Generic;

namespace Engine.Drawing
{
    public interface IUIProvider
    {
        IEnumerable<UIRequest> GetDrawings();
    }
}
