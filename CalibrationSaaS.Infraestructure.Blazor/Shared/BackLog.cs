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
using Helpers.Controls.ValueObjects;
using Helpers.Controls;

namespace CalibrationSaaS.Infraestructure.Blazor
{
    public class BackLogBase
        : Base_Create<WorkOrderDetail, Func<dynamic, Application.Services.IWorkOrderDetailServices<CallContext>>, Domain.Aggregates.Shared.Basic.AppState>
    //, IPage<WorkOrderDetail, IWorkOrderDetailServices, Domain.Aggregates.Shared.AppStateCompany>
    {
        public int Total { get; set; }

        public int Late { get; set; }

        public int DueToday { get; set; }

        public int NextDay { get; set; }

        public int TwoDay { get; set; }

        public int ThreeDay { get; set; }

        public int FourDay { get; set; }

        public string WoDId { get; set; }

        public ResponsiveTable<WorkOrderDetail> Grid { get; set; }


        [Parameter]
        public ICollection<WorkOrderDetail> List { get; set; } = new List<WorkOrderDetail>();


        public Search<WorkOrderDetail, IWorkOrderDetailServices<CallContext>, AppStateCompany> searchComponent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool First { get; set; }

        public override async Task<ResultSet<WorkOrderDetail>> LoadData(Pagination<WorkOrderDetail> pag)
        {
            WorkOrderDetailGrpc assets = new WorkOrderDetailGrpc(Client, DbFactory);
            pag.JSonDefinitionResult = "xxccvvccfdcccc";
            var Eq = (await assets.GetWods(pag, new CallContext()));

            if (!First)
            {
                Total = Eq.Count;
                First = true;
            }

            return Eq;
        }

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

                //var result = await Client.Delete(w);

                //WorkOrder_Create._listWorkOrderDetail.Remove(DTO);

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


        public ReportView report { get; set; }
        protected override async Task OnInitializedAsync()
        {

            eq = new WorkOrderDetail();

            eq.PieceOfEquipment = new PieceOfEquipment();

            eq.PieceOfEquipment.EquipmentTemplate = new EquipmentTemplate();

            eq.CurrentStatus = new Domain.Aggregates.Entities.Status();

            eq.PieceOfEquipment.Customer = new Customer();

            eq.TestCode = new TestCode();

            eq.WorkOder = new WorkOrder();

            WorkOrderDetailGrpc client = new WorkOrderDetailGrpc(Client, DbFactory);
            
            var res = await client.GetWOCountPerDay(new CallOptions());
            
            if (res != null && res.Count() > 0)
            {
                var tol = res.FirstOrDefault();

                int sdi = 0;
                //Total = tol.Value;
                //sumtemp = 0;
                //var late= res.Where(x => x.Key.Date < DateTime.Now.Date && tol.Key.Date != x.Key.Date).Sum(suma);

                var late = res.Where(x => x.Key.Date < DateTime.Now.Date.AddDays(sdi)).Sum(suma);

                //if(late != null)
                //{
                Late = late;
                //}
                sumtemp = 0;
                var today = res.Where(x => x.Key.Date == DateTime.Now.Date.AddDays(sdi)).Sum(suma);

            //if (today != null)
            //    {
                    DueToday = today;
                // }
                sumtemp = 0;
                var next = res.Where(x => x.Key.Date == DateTime.Now.AddDays(1+ sdi).Date).Sum(suma);

                //if (next != null)
                //{
                    NextDay = next;
                //}
                sumtemp = 0;
                var two = res.Where(x => x.Key.Date == DateTime.Now.AddDays(2 + sdi).Date).Sum(suma);

                //if (two != null)
                //{
                    TwoDay = two;
                //}
                sumtemp = 0;
                var three = res.Where(x => x.Key.Date == DateTime.Now.AddDays(3 + sdi).Date).Sum(suma);

                //if (three != null)
                //{
                ThreeDay = three;
                //}
                sumtemp = 0;
                var four = res.Where(x => x.Key.Date == DateTime.Now.AddDays(4+ sdi).Date).Sum(suma);

                //if (four != null)
                //{
                FourDay = four;
                //}


            }


            report= new ReportView();

            report.Columns = new List<ReportView.Column>();
            var c1 = new ReportView.Column();

            c1.Field = "WorkOrderDetailID";
            c1.Title = "ID";

            report.Columns.Add(c1);

            var c2 = new ReportView.Column();

            c2.Title = "WorkOrderID";

            c2.Field = "WorkOrderID";

            report.Columns.Add(c2);

            await  base.OnInitializedAsync();

            Component.Group = Component.Group.Replace("tech.HasView","");

            Component.Group = Component.Group + "tech,job.HasView,job.HasEdit,job.HasSave";

        }

        int sumtemp = 0;
        int suma(KeyValueDate key)
        {
            sumtemp = sumtemp + key.Value;

            return sumtemp;
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
