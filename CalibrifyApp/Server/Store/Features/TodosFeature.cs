﻿using Fluxor;
using Helpers.Controls.ValueObjects;
using System.Collections.Generic;
using CalibrifyApp.Server.Store.State;

namespace CalibrifyApp.Server.Store.Features
{
    public class TodosFeature : Feature<CalibrifyApp.Server.Store.State.TodosState>
    {
        public override string GetName() => "Todos";

        protected override CalibrifyApp.Server.Store.State.TodosState GetInitialState()
        {
            return new CalibrifyApp.Server.Store.State.TodosState(false, null, null, null, null, null);
        }
    }
}
