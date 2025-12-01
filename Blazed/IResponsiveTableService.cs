
using Helpers.Controls;

namespace Blazed.Controls
{
    public interface IResponsiveTableService
    {
        dynamic ResultState { get; set; }

        dynamic GetCache();
        
    }
}