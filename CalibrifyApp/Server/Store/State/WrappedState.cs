﻿using Fluxor;
using System;

namespace CalibrifyApp.Server.Store.State
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
