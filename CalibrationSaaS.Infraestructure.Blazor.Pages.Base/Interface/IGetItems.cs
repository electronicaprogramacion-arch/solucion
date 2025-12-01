using CalibrationSaaS.Domain.Aggregates.Entities;
using Helpers.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Base.Interfaces
{
    public interface IGetItems<Entity,CalibrationItem>
    {
        //public CreateModel Parameter { get; set; }
        public Task<List<CalibrationItem>> GetItems(Entity eq,int CalibrationSubTypeId, int ihneritCalSubtypeId);
      

    }
}