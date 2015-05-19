using System.Collections.Generic;

namespace Engine.Drawing.UI
{
    public interface IUIProvider
    {
        IEnumerable<UIRequest> GetDrawings();
    }
}
