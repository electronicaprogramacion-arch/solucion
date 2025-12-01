using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace CalibrifyApp.Server.Services
{
    // This class implements the Blazed.Controls.BrowserService interface
    // and delegates to our ServerBrowserService implementation
    public class BrowserServiceWrapper : Blazed.Controls.BrowserService
    {
        private readonly ServerBrowserService _serverBrowserService;

        public BrowserServiceWrapper(IJSRuntime js) : base(js)
        {
            _serverBrowserService = new ServerBrowserService(js);
        }

        public new async Task<Blazed.Controls.BrowserDimension> GetDimensions()
        {
            var serverDimension = await _serverBrowserService.GetDimensions();
            
            // Convert from our BrowserDimension to Blazed.Controls.BrowserDimension
            return new Blazed.Controls.BrowserDimension
            {
                Width = serverDimension.Width,
                Height = serverDimension.Height,
                Online = serverDimension.Online,
                Scroll = serverDimension.Scroll
            };
        }

        public async Task EnableLog()
        {
            await _serverBrowserService.EnableLog();
        }
    }
}
