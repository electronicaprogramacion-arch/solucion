﻿using Fluxor;
using System;

namespace CalibrationSaaS.Infraestructure.Blazor
{
    public class WrappedState<T> : IState<T>
    {
        private readonly T _value;

        public WrappedState(T initialValue)
        {
            _value = initialValue;
        }

        public T Value => _value;

        public event EventHandler StateChanged;

        public void NotifyStateChanged()
        {
            StateChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
