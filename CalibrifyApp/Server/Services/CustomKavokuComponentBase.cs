using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazed.Controls.Toast;

namespace CalibrifyApp.Server.Services
{
    /// <summary>
    /// Custom implementation of KavokuComponentBase that provides a default AuthenticationState.
    /// </summary>
    public class CustomKavokuComponentBase : CustomControlComponentBase
    {
        [Parameter]
        public new Component Component { get; set; } = new Component();

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Inject]
        public IToastService Toast { get; set; }

        public bool Saving { get; set; }
        public bool Loading { get; set; }
        public bool VisibleOverlay { get; set; }

        public async Task ShowProgress(bool focus = true)
        {
            if (JSRuntime != null)
            {
                await JSRuntime.InvokeVoidAsync("showProgressBar", "progressBar", focus);
            }
            VisibleOverlay = true;
        }

        public async Task CloseProgress()
        {
            if (JSRuntime != null)
            {
                await JSRuntime.InvokeVoidAsync("hideProgress", "progressBar");
            }
            VisibleOverlay = false;
            Saving = false;
            Loading = false;
        }

        public async Task ShowError(string message)
        {
            await ShowToast(message, ToastLevel.Error);
            await CloseProgress();
        }

        public async Task ShowToast(string message, ToastLevel level)
        {
            if (level == ToastLevel.Success)
            {
                Toast.ShowSuccess(message);
            }
            else if (level == ToastLevel.Error)
            {
                Toast.ShowError(message);
            }
            else if (level == ToastLevel.Info)
            {
                Toast.ShowInfo(message);
            }
            else if (level == ToastLevel.Warning)
            {
                Toast.ShowWarning(message);
            }
        }
    }
}
