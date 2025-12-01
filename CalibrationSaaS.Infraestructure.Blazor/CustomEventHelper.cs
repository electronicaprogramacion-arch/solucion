using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor
{
    public class CustomEventHelper
    {
        private readonly Func<EventArgs, Task> _callback;

        public CustomEventHelper(Func<EventArgs, Task> callback)
        {
            _callback = callback;
        }

        [JSInvokable]
        public Task OnCustomEvent(EventArgs args) => _callback(args);
    }

    public class CustomEventInterop : IDisposable
    {
        private readonly IJSRuntime _jsRuntime;

        private DotNetObjectReference<CustomEventHelper> Reference;


        public CustomEventInterop(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public ValueTask<string> SetupCustomEventCallback(Func<EventArgs, Task> callback)
        {
            //Reference = DotNetObjectReference.Create(new ScrollEventHelper(callback));
            //// addCustomEventListener will be a js function we create later
            return _jsRuntime.InvokeAsync<string>("addCustomEventListener", Reference);



        }

        public void Dispose()
        {
            Reference?.Dispose();
        }
    }
}
