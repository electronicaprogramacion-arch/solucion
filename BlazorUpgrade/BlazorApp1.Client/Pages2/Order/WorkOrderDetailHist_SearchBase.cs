using CalibrationSaaS.Application.Services;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Shared;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Helpers.Controls.ValueObjects;
using Microsoft.AspNetCore.Components;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorApp1.Blazor.Blazor.Pages.Order
{
    public class WorkOrderDetailHist_SearchBase
        : Base_Create<WorkDetailHistory, IWorkOrderDetailServices<CallContext>, CalibrationSaaS.Domain.Aggregates.Shared.AppStateCompany>
    //, IPage<WorkOrderDetail, IWorkOrderDetailServices, Domain.Aggregates.Shared.AppStateCompany>
    {




        public ResponsiveTable<WorkDetailHistory> Grid { get; set; }


        [Parameter]
        public ICollection<WorkDetailHistory> List { get; set; } = new List<WorkDetailHistory>();



        public Task Delete(CalibrationSaaS.Domain.Aggregates.Entities.WorkOrderDetail DTO)
        {
            throw new NotImplementedException();
        }


        public Task SelectModal(WorkOrderDetail DTO)
        {
            throw new NotImplementedException();
        }


        [Parameter]
        public WorkOrderDetail WorkOrderDetail { get; set; }

        public List<WorkDetailHistory> HistoryList { get; set; }

        [Parameter]
        public List<WorkDetailHistory> LIST1 { get; set; }

        public override async Task<ResultSet<WorkDetailHistory>> LoadData(Pagination<WorkDetailHistory> pag)
        {
            ResultSet<WorkDetailHistory> result = new ResultSet<WorkDetailHistory>();
            //if (IsForEt == false)

            //{
            //    _poeGrpc = new PieceOfEquipmentGRPC(Client);

            //    var g = new CallOptions(await GetHeaderAsync());

            //    result = (await _poeGrpc.GetPiecesOfEquipmentXDueDate(pag, Header));

            //}
            WorkOrderDetail w = new WorkOrderDetail();

            WorkOrderDetailGrpc wod = new WorkOrderDetailGrpc(Client);


            var a = await wod.GetHistory(WorkOrderDetail);

            result.List = a.WorkOrderDetailsHistory;

            LIST1 = result.List;

            return result;



        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            
            
            await  base.OnAfterRenderAsync(firstRender);




        }

        public string wd = "";

        protected override async Task OnInitializedAsync()
        {
            
            await  base.OnInitializedAsync();


            ResultSet<WorkDetailHistory> result = new ResultSet<WorkDetailHistory>();

            WorkOrderDetail w = new WorkOrderDetail();

            WorkOrderDetailGrpc wod = new WorkOrderDetailGrpc(Client);


            var a = await wod.GetHistory(WorkOrderDetail);

            result.List = a.WorkOrderDetailsHistory;

            LIST1 = result.List;

            if(LIST1 != null && LIST1.Count > 0)
            {
                wd="Work Order Detail ID:  "+ LIST1[0].WorkOrderDetailID.ToString();
            }


        }

    }
}
