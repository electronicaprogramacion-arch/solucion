using Blazed.Controls;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Extensions.Configuration;

namespace BlazorApp1.Blazor.Shared
{
    public class NavMenu_Base:ComponentBase
    {

          [Inject]
          public  BrowserService BrowserService { get; set; }

        [Inject]
        public IConfiguration Configuration { get; set; }

        private string infoString { get; set; }
        public string isdemo { get; set; }
        private void ShowInfo()
        {
            infoString = "Is admin: False"; // Always False
        }



        protected bool CanShow { get; set; } = true;

        public async Task<BrowserDimension> GetDimensions()
        {

            try
            {


                var dimension = await BrowserService.GetDimensions();

                return dimension;
                //Height = dimension.Height;
                //Width = dimension.Width;
            }
            catch (Exception ex)
            {

                throw ex;


            }

        }
        public string Customer { get; set; }
        protected async override Task OnInitializedAsync()
        {


            

        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var a = await Show();
                if (a)
                {
                    CanShow = true;
                }
                else
                {
                    CanShow = false;
                }
                ConnectionStatusService.OnChange += DecideMenuElementsToshow;
                isdemo = Configuration.GetSection("Reports")["IsDemo"];
                Customer = Configuration.GetSection("Reports")["Customer"];
            }
         

        }


        private async Task DecideMenuElementsToshow(bool connectionStus)
        {
            await InvokeAsync(async () =>
            {
                var a = await Show();

                var CanShow2 = connectionStus;

                if (a && CanShow2)
                {
                    CanShow = true;
                }
                else
                {
                    CanShow = false;
                }

                StateHasChanged();

            });
        }

        public async Task<bool> Show()
        {

            try
            {
                var a = await GetDimensions();


                if (a.Width < 560)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("error dimension");
                return true;
            }


        }
    }
}
