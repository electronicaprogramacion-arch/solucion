
using Helpers.Controls;

namespace Blazor.Controls
{
    public interface IResponsiveTableService
    {
        dynamic ResultState { get; set; }

        dynamic GetCache();
        
    }
}