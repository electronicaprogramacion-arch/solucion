using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Store.Features.Todos.Actions.LoadTodos;
using Fluxor;
using Helpers.Controls.ValueObjects;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
//using StateManagementWithFluxor.Store.Features.Todos.Actions.LoadTodoDetail;
//using StateManagementWithFluxor.Store.Features.Todos.Actions.LoadTodos;



namespace CalibrationSaaS.Infraestructure.Blazor
{


    public class StateFacade : IStateFacade
    {
        private readonly ILogger<StateFacade> _logger;
        private readonly IDispatcher _dispatcher;

        public StateFacade(ILogger<StateFacade> logger, IDispatcher dispatcher) =>
            (_logger, _dispatcher) = (logger, dispatcher);


        public void LoadTodos2<T>(ResultSet<T> Data, Pagination<T> _parameter, string Route)
        {
            //var par = _parameter.Entity.GetType().Name;

            //_logger.LogInformation("Data Issuing action to load todos...");
            var a = new LoadTodosAction(Data, _parameter);
            a.Url = Route;
            // a.LoadData<T>(Data);
            _dispatcher.Dispatch(a);
        }

        public void LoadTodos<T>(Func<Pagination<T>, Task<ResultSet<T>>> _action, Pagination<T> _parameter)
        {
            object obj1 = new object();
            object obj2 = new object();

            //_logger.LogInformation("Issuing action to load todos...");
            _dispatcher.Dispatch(new LoadTodosAction(_action, _parameter));
        }

        public void LoadResult<T>(Func<Pagination<T>, Task<ResultSet<T>>> _action, Pagination<T> _parameter)
        {
            //_logger.LogInformation("Issuing action to load todos...");

            //_dispatcher.Dispatch(new LoadAction<T>(_action, _parameter));
        }


    }
}

