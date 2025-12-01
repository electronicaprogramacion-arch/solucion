using CalibrationSaaS.Store.Features.Todos.Actions.LoadTodos;

using Fluxor;
using Helpers.Controls;

namespace CalibrationSaaS.Store.Features.Todos.Reducers
{
    public static class LoadTodosActionsReducer
    {
        //[ReducerMethod]
        //public static TodosState ReduceLoadTodosAction(TodosState state, LoadTodosAction action) =>
        //    new TodosState(true, null, action.Url, null, state.CurrentTodo, action.Pagination);

        //[ReducerMethod]
        //public static TodosState ReduceLoadTodosSuccessAction(TodosState state, LoadTodosSuccessAction action) =>
        //    new TodosState(false, null, action.Url, action.Todos, state.CurrentTodo, action.Pagination);

        //[ReducerMethod]
        //public static TodosState ReduceLoadTodosFailureAction(TodosState state, LoadTodosFailureAction action) =>
        //    new TodosState(false, action.ErrorMessage, null, null, state.CurrentTodo, null);

        //[ReducerMethod]
        //public static PaginationState ReduceLoadTodosAction(PaginationState state, LoadTodosAction action) =>
        //    state with { pagination = action.Pagination };

        //[ReducerMethod]
        //public static TodosState ReduceLoadTodosSuccessAction(TodosState state, LoadTodosSuccessAction action) =>
        //    new TodosState(false, null, action.Url, action.Todos, state.CurrentTodo, action.Pagination);

        //[ReducerMethod]
        //public static TodosState ReduceLoadTodosFailureAction(TodosState state, LoadTodosFailureAction action) =>
        //    new TodosState(false, action.ErrorMessage, null, null, state.CurrentTodo, null);

        [ReducerMethod]
        public static TodosState ReduceLoadTodosAction(TodosState state, LoadTodosAction action)
        {



            //return new TodosState(true, null, action.Url, null, state.CurrentTodo, null);

            state.IsLoading = true;
            state.CurrentErrorMessage = null;
            state.Url = action.Url;
            state.CurrentTodo = null;
            state.CurrentPagination = null;

            return state;

        }


        [ReducerMethod]
        public static TodosState ReduceLoadTodosSuccessAction(TodosState state, LoadTodosSuccessAction action)
        {


            state.IsLoading = false;
            state.CurrentErrorMessage = null;
            //state.Url = action.Url;
            state.CurrentTodo = null;
            state.CurrentPagination = action.Pagination;
            //var state2 =new TodosState(false, null, action.Url, action.Todos, state.CurrentTodo, action.Pagination);
            var result = state.AddPagination(action.Pagination);
            state.Url = result;
            return state;

            //return new TodosState(false, null, action.Url, action.Todos, state.CurrentTodo, action.Pagination);


        }


        [ReducerMethod]
        public static TodosState ReduceLoadTodosFailureAction(TodosState state, LoadTodosFailureAction action) =>
            new TodosState(false, action.ErrorMessage, null, null, state.CurrentTodo, null);



    }
}
