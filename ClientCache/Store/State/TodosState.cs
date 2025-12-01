
using Blazor.Controls.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Store.State
{

    public class TodosStateDic
    {
        public string Key { get; set; }

        public object Value { get; set; }
    }

    public class TodosState : RootState
    {
        public List<TodosStateDic> _dicState { get; set; } = new List<TodosStateDic>();

        public TodosState(bool isLoading, string? currentErrorMessage, string url, dynamic currentTodos, TodoDto currentTodo, object pagination)

            : base(isLoading, currentErrorMessage, url)
        {
            CurrentTodos = currentTodos;
            CurrentTodo = currentTodo;
            CurrentPagination = pagination;
            //CurrentType = type;
        }

        public dynamic CurrentTodos { get; set; }

        public TodoDto CurrentTodo { get; set; }

        public object CurrentPagination { get; set; }

        public string CurrentType { get; set; } = "test";

        public TodosState Add(TodosState todosState)
        {
            var route = ((IPaginationBase)(todosState.CurrentPagination)).Component.Route;

            //_dicState.Add(((IPaginationBase)(todosState.CurrentPagination)).Component.Route, todosState);

            var dd = _dicState.Where(x => x.Key == route).FirstOrDefault();

            if (dd != null)
            {
                //var r = _dicState.GetValueOrDefault(route);

                //_dicState.Remove(route);

//                Console.WriteLine("Add dictionary " + route);

//                Console.WriteLine(((IPaginationBase)(todosState.CurrentPagination)).Component.Route);

                _dicState.Remove(dd);

                //return r;

            }
            else
            {
//                Console.WriteLine("Add dictionary2 " + ((IPaginationBase)(todosState.CurrentPagination)).Component.Route);
                //_dicState.Add(route, todosState);

            }
            TodosStateDic dic = new TodosStateDic();
            dic.Key = route;
            dic.Value = todosState;
            _dicState.Add(dic);

            //todosState.CurrentPagination = null;

            return todosState;

        }


        public string AddPagination(object CurrentPagination)
        {
            var route = ((IPaginationBase)(CurrentPagination)).Component.Route;

            route = route.Replace("?h=y","");
            //_dicState.Add(((IPaginationBase)(todosState.CurrentPagination)).Component.Route, todosState);

            var dd = _dicState.Where(x => x.Key == route).FirstOrDefault();

            if (dd != null)
            {
                //var r = _dicState.GetValueOrDefault(route);

                //_dicState.Remove(route);

//                Console.WriteLine("Add dictionary " + route);

//                Console.WriteLine(((IPaginationBase)(CurrentPagination)).Component.Route);

                _dicState.Remove(dd);

                //return r;

            }
            else
            {
//                Console.WriteLine("Add dictionary2 " + ((IPaginationBase)(CurrentPagination)).Component.Route);
                //_dicState.Add(route, todosState);

            }
            TodosStateDic dic = new TodosStateDic();
            dic.Key = route;
            dic.Value = CurrentPagination;
            _dicState.Add(dic);

            //todosState.CurrentPagination = null;

            return route;

        }

    }
}
