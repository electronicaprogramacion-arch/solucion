
using Fluxor;
using Helpers.Controls;

namespace CalibrationSaaS.Store.Features.Todos
{

    public class TodosFeature : Feature<TodosState>
    {
        public override string GetName() => "Todos";

        protected override TodosState GetInitialState() =>
            new TodosState(false, null, null, null, null, null);
    }
}
