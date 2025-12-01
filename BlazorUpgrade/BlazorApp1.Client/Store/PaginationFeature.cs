using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Fluxor;
using Helpers.Controls.ValueObjects;

namespace CalibrationSaaS.Store.Features.Todos
{

    public record PaginationState(IPaginationBase pagination);
    public class PaginationFeature : Feature<PaginationState>
    {
        public override string GetName() => "Pagination";

        protected override PaginationState GetInitialState() =>
            new PaginationState(null);
    }
}
