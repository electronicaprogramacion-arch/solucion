using CalibrationSaaS.Application.Services;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Shared;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using Grpc.Core;
using Microsoft.AspNetCore.Components;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Blazored.Modal;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.AssetsBasics
{
    public class WorkOrderDetailChildren_SearchBase
        : Base_Create<WorkOrderDetail,Func<dynamic, IWorkOrderDetailServices<CallContext>>, Domain.Aggregates.Shared.AppStateCompany>
    //, IPage<WorkOrderDetail, IWorkOrderDetailServices, Domain.Aggregates.Shared.AppStateCompany>
    {


        public ModalParameters parameters { get; set; } = new ModalParameters();

        [Parameter]
        public ICollection<WorkOrderDetail> _listWorkOrderDetail { get; set; } = new List<WorkOrderDetail>();
        //[CascadingParameter]
        //public WorkOrder_Create WorkOrder_Create { get; set; }

        public ResponsiveTable<WorkOrderDetail> Grid { get; set; }


        [Parameter]
        public ICollection<WorkOrderDetail> List { get; set; } = new List<WorkOrderDetail>();


        public Search<WorkOrderDetail, IWorkOrderDetailServices<CallContext>, AppStateCompany> searchComponent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
      

        public async Task<bool> Delete(Domain.Aggregates.Entities.WorkOrderDetail DTO)
        {
            try
            {
                //await searchComponent.ShowModalAction();

                if (DTO.CurrentStatusID > 1)
                {
                    throw new Exception("Work Order Detail is being used, Only Work Order Detail in contract review status can be deleted ");
                }

                WorkOrderDetail w = new WorkOrderDetail();

                w.WorkOrderDetailID = DTO.WorkOrderDetailID;

                WorkOrderDetailGrpc cl = new WorkOrderDetailGrpc(Client,DbFactory);

                var result = await cl.Delete(w, new CallContext());

                _listWorkOrderDetail.Remove(DTO);

                return true;
                // searchComponent.ShowResult();
            }
            catch (RpcException ex)
            {

                await ExceptionManager(ex);
                return false;

            }
            catch (Exception ex)
            {
                await ExceptionManager(ex);
                return false;

            }
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

        public async Task CloseAddChild(ChangeEventArgs arg)
        {

        }

    }
}
