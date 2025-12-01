
using Blazor.Controls.Route.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Controls
{
    public partial class BackComponent
    {
        [Inject]
        public RouterSessionService RouterSessionService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public bool Disabled { get; set; } = true;

        protected override Task OnParametersSetAsync()
        {
            return base.OnParametersSetAsync();
        }

        protected override Task OnInitializedAsync()
        {


            return base.OnInitializedAsync();
        }

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task Back()
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {

            var router = RouterSessionService.ActiveComponent;

            var last = RouterSessionService.LastRouteUrl;

            var History = RouterSessionService.History;

            if (History != null && History.Count > 1)
            {
                var inddd = 0;
                //if (History.Count > 2)
                if (RouterSessionService.CurrentNavigationIndex == -1 && History.Count > 2)

                {
                    inddd = History.Count - 2;
                }
                else if (RouterSessionService.CurrentNavigationIndex == -1)
                {
                    inddd = 1;
                }
                else
                {
                    inddd = RouterSessionService.CurrentNavigationIndex - 1;
                }

                RouterSessionService.CurrentNavigationIndex = inddd;
//                Console.WriteLine(RouterSessionService.CurrentNavigationIndex);
//                Console.WriteLine(History.Count);
//                Console.WriteLine(inddd);
                if (inddd > 0)
                {
                    //RouterSessionService.CurrentNavigationIndex = inddd;
                    //var h= History.ElementAtOrDefault(RouterSessionService.CurrentNavigationIndex);
                    var h = History.ElementAtOrDefault(inddd);
                    RouterSessionService.HistoryButton = true;
                    if (h.Route.Contains("?h=y"))
                    {
                        NavigationManager.NavigateTo(h.Route);
                    }
                    else
                    {
                        NavigationManager.NavigateTo(h.Route + "?h=y");
                    }

                }
                //else if(RouterSessionService.CurrentNavigationIndex > 0)
                //{
                //    var inn = RouterSessionService.CurrentNavigationIndex - 1;
                //    var h = History.ElementAtOrDefault(inddd);
                //    RouterSessionService.HistoryButton = true;
                //    NavigationManager.NavigateTo(h.Route + "?h=y");
                //}

                Disabled = false;

            }








        }



#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task Forward()
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {

            var router = RouterSessionService.ActiveComponent;

            var last = RouterSessionService.LastRouteUrl;

            var History = RouterSessionService.History;


            RouterSessionService.HistoryButton = true;
            NavigationManager.NavigateTo(RouterSessionService.ReturnRouteUrl);


            return;

            //if (History != null && History.Count > 1)
            //{

            //    if (RouterSessionService.CurrentNavigationIndex == -1)
            //    {
            //        RouterSessionService.CurrentNavigationIndex = History.Count - 2;
            //    }
            //    else
            //    {
            //        RouterSessionService.CurrentNavigationIndex = RouterSessionService.CurrentNavigationIndex + 1;
            //    }

            //    if (RouterSessionService.CurrentNavigationIndex >= 0 && RouterSessionService.CurrentNavigationIndex < (History.Count -1))
            //    {
            //        var h = History.ElementAtOrDefault(RouterSessionService.CurrentNavigationIndex);

            //        RouterSessionService.HistoryButton = true;
            //        NavigationManager.NavigateTo(h.Route + "?h=y");
            //    }

            //}




        }

    }
}
