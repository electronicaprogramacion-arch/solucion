using Microsoft.JSInterop;
using System.Reflection;
using System.Threading.Tasks;

namespace CalibrifyApp.Server.Services
{
    /// <summary>
    /// Server-side implementation of LazyAssemblyLoader
    /// This is a mock implementation that doesn't actually load assemblies dynamically
    /// since server-side doesn't support this in the same way as WebAssembly
    /// </summary>
    public class ServerLazyAssemblyLoader
    {
        private readonly IJSRuntime _jsRuntime;

        public ServerLazyAssemblyLoader(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public Task<IEnumerable<Assembly>> LoadAssembliesAsync(IEnumerable<string> assemblyNames)
        {
            // In a server-side context, we don't need to dynamically load assemblies
            // This is just a mock to satisfy the dependency
            return Task.FromResult<IEnumerable<Assembly>>(Array.Empty<Assembly>());
        }
    }
}
