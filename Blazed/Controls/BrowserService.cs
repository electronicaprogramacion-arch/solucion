using Microsoft.JSInterop;
using System.Threading.Tasks;


namespace Blazed.Controls
{
    public class BrowserService
    {
        private readonly IJSRuntime _js;

        public BrowserService(IJSRuntime js)
        {
            _js = js;
        }

        public async Task<BrowserDimension> GetDimensions()
        {
            if (_js != null)
            {
                return await _js.InvokeAsync<BrowserDimension>("getDimensions");
            }
            return null;
        }

        //public async Task EnableLog()
        //{
        //    await _js.InvokeVoidAsync("setLog");
        //}


    }

    public class BrowserDimension
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public bool Online { get; set; }

        public int Scroll { get; set; } = 0;

        public bool Install { get; set; }
    }

}