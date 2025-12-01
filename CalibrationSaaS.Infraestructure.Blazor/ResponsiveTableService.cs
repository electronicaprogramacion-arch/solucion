

using Fluxor;
using Helpers.Controls;

namespace Blazed.Controls
{
    public class ResponsiveTableService : IResponsiveTableService
    {


        public ResponsiveTableService(IState<TodosState> _State)
        {
            this.ResultState = _State;
        }

        public dynamic ResultState { get; set; }

       

        public dynamic GetCache()
        {
            return ResultState;
        }

    }
}
