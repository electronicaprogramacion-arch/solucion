
using Helpers.Controls.ValueObjects;

using Fluxor;

namespace Store.Features.Todos
{

    public record PaginationState(IPaginationBase pagination);
    public class PaginationFeature : Feature<PaginationState>
    {
        public override string GetName() => "Pagination";

        protected override PaginationState GetInitialState() =>
            new PaginationState(null);
    }
}
