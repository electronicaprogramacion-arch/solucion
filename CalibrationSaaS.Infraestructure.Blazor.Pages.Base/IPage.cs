using CalibrationSaaS.Domain.Aggregates.Entities;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Base
{
    public interface IPage<T, S, D> where T : IGeneric, new()
    {

        public Search<T, S, D> searchComponent { get; set; }

        public IEnumerable<T> FilterList(string filter);

        public Task SelectModal(T DTO);

        public Task Delete(T DTO);

        [Parameter]
        public bool SelectOnly { get; set; }

        [Parameter]
        public bool IsModal { get; set; }


    }
}
