﻿using Blazed.Controls;
using CalibrifyApp.Server.Store.State;
using Fluxor;

namespace CalibrifyApp.Server.Store
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
