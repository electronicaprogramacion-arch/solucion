using Grpc.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Base
{
    public class LayoutBase : FComponentBase // Fluxor.Blazor.Web.Components.FluxorLayout //LayoutComponentBase
    {

        internal const string BodyPropertyName = "Body";

        //
        // Resumen:
        //     Gets the content to be rendered inside the layout.
        [Parameter]
        public RenderFragment? Body { get; set; }

        //[DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(LayoutComponentBase))]
        //public override Task SetParametersAsync(ParameterView parameters)
        //{
        //    return base.SetParametersAsync(parameters);
        //}



        public bool Saving { get; set; }

        public bool Loading { get; set; }

        [Inject] public ILogger<LayoutBase> Logger { get; set; }
        [Inject] public IJSRuntime JSRuntime { get; set; }

        public bool IsClosed { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("removeValidClass");
            }

            

            await base.OnAfterRenderAsync(firstRender);

        }


        public virtual async Task CloseModal(object result = null)
        {
            IsClosed = true;

            await CloseProgress();


        }



        public async Task ExceptionManager(RpcException rpcException)
        {
            string Error = "";

            //|| rpcException.StatusCode == StatusCode.Unauthenticated
            //   || rpcException.StatusCode == StatusCode.Cancelled
            //   || rpcException.StatusCode == StatusCode.AlreadyExists


            if (rpcException.StatusCode > 0)
            {
                Error = rpcException.Message;
            }
            else
            {
                Error = rpcException.Status.Detail;
            };
            if (!string.IsNullOrEmpty(Error) && Error.Contains("|"))
            {
                var a = Error.Split("|");

                //foreach (var it in a)
                //{
                //    await ShowError(it);
                //}

            }
            else

                if (!string.IsNullOrEmpty(Error))
            {
                //Logger.LogError(Error);
                //await ShowError(Error);
            }
            else
            {
                //await ShowError("Error in operation");
            }
            Saving = false;
            Loading = false;
            await CloseProgress();
        }


        public async Task ExceptionManager(Exception rpcException)
        {
            string Error = "";

            Error = rpcException.Message;

            if (!string.IsNullOrEmpty(Error) && Error.Contains("|"))
            {
                var a = Error.Split("|");

                //foreach (var it in a)
                //{
                //    await ShowError(it);
                //}

            }
            else

           if (!string.IsNullOrEmpty(Error))
            {

                //await ShowError(Error);
                //Logger.LogError(Error);
                //Toast.ShowError(Error);
            }
            Saving = false;
            Loading = false;
            await CloseProgress();
        }

        public async Task CloseProgress(bool excecuteScript = true)
        {
            if (JSRuntime != null && excecuteScript)
            {
                //Console.WriteLine("CloseProgress layout base");
                await JSRuntime.InvokeVoidAsync("hideProgress", "progressBar");
            }
            Saving = false;
            Loading = false;

        }

        public async Task ShowProgress(bool excecuteScript = true)
        {
            if (JSRuntime != null && excecuteScript)
            {
                await JSRuntime.InvokeVoidAsync("showProgressBar", "progressBar");
            }
            Saving = true;
            Loading = true;
        }
    }




}
