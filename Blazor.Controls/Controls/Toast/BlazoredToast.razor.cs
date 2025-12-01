using Blazored.Toast;
using Blazor.Controls;
using Blazor.Controls;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Blazor.Controls
{
    public partial class BlazoredToast : IDisposable
    {
        [Inject]
        public BrowserService BrowserService { get; set; }

        [CascadingParameter] private BlazoredToasts ToastsContainer { get; set; }
        [Parameter] public int CurrentPosition { get; set; }
        [Parameter] public Guid ToastId { get; set; }
        [Parameter] public ToastSettings ToastSettings { get; set; }
        [Parameter] public int Timeout { get; set; } = 1000;

        //[Inject] IJSRuntime JSRuntime { get; set; }

        private CountdownTimer _countdownTimer;
        private int _progress = 100;

        public int topScroll { get; set; } = 0;


        protected override async Task OnInitializedAsync()
        {
            //JSRuntime.InvokeVoidAsync("showToast", ToastId);

            var dimension = await BrowserService.GetDimensions();

            topScroll = dimension.Scroll + 50;

//            Console.WriteLine("topScroll " +topScroll);

            _countdownTimer = new CountdownTimer(Timeout);
            _countdownTimer.OnTick += CalculateProgress;
            _countdownTimer.OnElapsed += () => { Close(); };
            _countdownTimer.Start();


        }

        //protected override void OnInitialized()
        //{
        //    //JSRuntime.InvokeVoidAsync("showToast", ToastId);

        //    var dimension =  BrowserService.GetDimensions().ConfigureAwait(false).GetAwaiter().GetResult();

        //    topScroll = dimension.Scroll;

        //    _countdownTimer = new CountdownTimer(Timeout);
        //    _countdownTimer.OnTick += CalculateProgress;
        //    _countdownTimer.OnElapsed += () => { Close(); };
        //    _countdownTimer.Start();

        //}

        private async void CalculateProgress(int percentComplete)
        {
            _progress = 100 - percentComplete;
            await InvokeAsync(StateHasChanged);
        }

        private void Close()
        {
            ToastsContainer.RemoveToast(ToastId);
        }

        public void Dispose()
        {
            _countdownTimer.Dispose();
            _countdownTimer = null;
        }
    }
}