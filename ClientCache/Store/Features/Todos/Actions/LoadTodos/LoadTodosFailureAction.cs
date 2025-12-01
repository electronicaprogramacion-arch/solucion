using Store.Features.Shared.Actions;

namespace Store.Features.Todos.Actions.LoadTodos
{
    public class LoadTodosFailureAction : FailureAction
    {
        public LoadTodosFailureAction(string errorMessage)
            : base(errorMessage)
        {
        }
    }
}
