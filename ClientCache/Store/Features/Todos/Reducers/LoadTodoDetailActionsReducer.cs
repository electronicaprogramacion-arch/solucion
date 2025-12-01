using Fluxor;
using StateManagementWithFluxor.Models.Todos;
using StateManagementWithFluxor.Store.Features.Todos.Actions.LoadTodoDetail;
using StateManagementWithFluxor.Store.State;

namespace StateManagementWithFluxor.Store.Features.Todos.Reducers
{
    public static class LoadTodoDetailActionsReducer
    {
        [ReducerMethod]
        public static TodosState<TodoDto> ReduceLoadTodoDetailAction(TodosState<TodoDto> state, LoadTodoDetailAction _) =>
            new TodosState<TodoDto>(true, null, state.CurrentTodos, null);

        [ReducerMethod]
        public static TodosState<TodoDto> ReduceLoadTodoDetailSuccessAction(TodosState<TodoDto> state, LoadTodoDetailSuccessAction action) =>
            new TodosState<TodoDto>(false, null, state.CurrentTodos, action.Todo);

        [ReducerMethod]
        public static TodosState<TodoDto> ReduceLoadTodoDetailFailureAction(TodosState<TodoDto> state, LoadTodoDetailFailureAction action) =>
            new TodosState<TodoDto>(false, action.ErrorMessage, state.CurrentTodos, null);
    }
}
