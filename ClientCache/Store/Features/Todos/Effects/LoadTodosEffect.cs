

using Blazor.Controls.ValueObjects;
using Store.Features.Todos.Actions.LoadTodos;
using Fluxor;
using Microsoft.Extensions.Logging;


using System;
using System.Threading.Tasks;

namespace Store.Features.Todos.Effects
{
    public class LoadTodosEffect : Effect<LoadTodosAction>
    {
        private readonly ILogger<LoadTodosEffect> _logger;
        //private readonly HttpClient _httpClient;

        public LoadTodosEffect(ILogger<LoadTodosEffect> logger)
        {
            _logger = logger;
        }


#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public override async Task HandleAsync(LoadTodosAction action, IDispatcher dispatcher)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            try
            {

                //// Use reflection to get the Method
                //var type = action.GetType();
                //var methodInfo = type.GetMethod("Method");

                //// Invoke the method here
                //List<object> args = new List<object>(); 
                //args.Add(action.Parameter);   

                //var a= methodInfo.Invoke(action, args.ToArray());

                //action.Method.GetType()
                // Add a little extra latency for dramatic effect...
                //await Task.Delay(TimeSpan.FromMilliseconds(1000));
                //var todosResponse = await _httpClient.GetFromJsonAsync<IEnumerable<object>>("todos");

                if (action.Data1 != null && action.Pagination != null)
                {
                    //_logger.LogInformation("Efect route: " + ((IPaginationBase)action.Pagination).Component.Route);
                    var todosResponse = action.Data1;
                    dispatcher.Dispatch(new LoadTodosSuccessAction(todosResponse, action.Pagination, action.Url));
                }


            }
            catch (Exception e)
            {

                dispatcher.Dispatch(new LoadTodosFailureAction(e.Message));
            }

        }
    }
}
