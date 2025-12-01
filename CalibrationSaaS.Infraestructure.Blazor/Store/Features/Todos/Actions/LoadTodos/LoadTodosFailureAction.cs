using CalibrationSaaS.Store.Features.Shared.Actions;

namespace CalibrationSaaS.Store.Features.Todos.Actions.LoadTodos
{
    public class LoadTodosFailureAction : FailureAction
    {
        public LoadTodosFailureAction(string errorMessage)
            : base(errorMessage)
        {
        }
    }
}
