using CalibrationSaaS.Application.Services;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Shared;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using Microsoft.AspNetCore.Components;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.AssetsBasics
{
    public class WorkOrderDetailHist_SearchBase
        : Base_Create<WorkOrderDetail, IWorkOrderDetailServices<CallContext>, Domain.Aggregates.Shared.AppStateCompany>
    //, IPage<WorkOrderDetail, IWorkOrderDetailServices, Domain.Aggregates.Shared.AppStateCompany>
    {


        public ResponsiveTable<WorkOrderDetail> Grid { get; set; }


        [Parameter]
        public ICollection<WorkOrderDetail> List { get; set; } = new List<WorkOrderDetail>();


        

        public Task Delete(Domain.Aggregates.Entities.WorkOrderDetail DTO)
        {
            throw new NotImplementedException();
        }


        public Task SelectModal(WorkOrderDetail DTO)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<WorkOrderDetail> FilterList(string filter = "")
        {

            if (Grid != null && Grid.ItemsDataSource != null)
            {
                var templist = Grid.ItemsDataSource;

                //return null;

                return templist.Where(i => i.Name.ToLower().Contains(filter.ToLower()

                    )).ToArray();
            }
            else
            {
                return null;
            }


        }

    }
}
