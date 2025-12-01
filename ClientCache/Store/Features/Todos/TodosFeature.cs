using Store.State;
using Fluxor;

namespace Store.Features.Todos
{

    public class TodosFeature : Feature<TodosState>
    {
        public override string GetName() => "Todos";

        protected override TodosState GetInitialState() =>
            new TodosState(false, null, null, null, null, null);
    }
}
