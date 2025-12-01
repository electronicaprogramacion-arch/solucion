﻿using Helpers.Controls.ValueObjects;
using System;
using System.Threading.Tasks;

namespace CalibrifyApp.Server.Store
{
    public interface IExtendedStateFacade : Blazed.Controls.IStateFacade
    {
        dynamic GetCache();
        dynamic GetCache(string route);
        void LoadResult<T>(dynamic func, dynamic pagination) where T : class;
    }
}
