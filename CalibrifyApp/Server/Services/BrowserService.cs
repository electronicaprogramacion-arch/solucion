using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace CalibrifyApp.Server.Services
{
    // This class is used to create a server-side implementation of Blazed.Controls.BrowserService
    public class ServerBrowserService
    {
        private readonly IJSRuntime _js;

        public ServerBrowserService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<BrowserDimension> GetDimensions()
        {
            return await _js.InvokeAsync<BrowserDimension>("getDimensions");
        }

        public async Task EnableLog()
        {
            await _js.InvokeVoidAsync("setLog");
        }
    }
}
