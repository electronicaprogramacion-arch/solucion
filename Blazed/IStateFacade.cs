using Helpers.Controls.ValueObjects;
using System;
using System.Threading.Tasks;

namespace Blazed.Controls
{
    public interface IStateFacade
    {
        void LoadResult<T>(Func<Pagination<T>, Task<ResultSet<T>>> _action, Pagination<T> _parameter);
        void LoadTodos<T>(Func<Pagination<T>, Task<ResultSet<T>>> _action, Pagination<T> _parameter);
        void LoadTodos2<T>(ResultSet<T> Data, Pagination<T> _parameter, string Route);
    }
}