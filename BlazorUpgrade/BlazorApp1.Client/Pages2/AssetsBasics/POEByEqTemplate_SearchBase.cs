using Blazed.Controls;
using CalibrationSaaS.Application.Services;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.Shared;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using Helpers.Controls.ValueObjects;
using Microsoft.AspNetCore.Components;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BlazorApp1.Blazor.Blazor.Pages.AssetsBasics
{
    public class POEByEqTemplate_SearchBase
        : Base_Create<PieceOfEquipment, IPieceOfEquipmentService<CallContext>, CalibrationSaaS.Domain.Aggregates.Shared.AppStateCompany>
    //, IPage<WorkOrderDetail, IWorkOrderDetailServices, Domain.Aggregates.Shared.AppStateCompany>
    {


        [Parameter]
        public bool IsCustomer { get; set; }

        public ResponsiveTable<PieceOfEquipment> Grid { get; set; }


        [Parameter]
        public ICollection<PieceOfEquipment> List { get; set; } = new List<PieceOfEquipment>();
        

        public Task Delete(CalibrationSaaS.Domain.Aggregates.Entities.PieceOfEquipment DTO)
        {
            throw new NotImplementedException();
        }


        public Task SelectModal(PieceOfEquipment DTO)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PieceOfEquipment> FilterList(string filter = "")
        {

            if (Grid != null && Grid.ItemsDataSource != null)
            {
                var templist = Grid.ItemsDataSource;

                //return null;

                return templist.Where(i => i.PieceOfEquipmentID.ToString().ToLower().Contains(filter.ToLower()

                    )).ToArray();
            }
            else
            {
                return null;
            }


        }



        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            //Component.Group = "admin,tech";

            Component.IsModal = true;

            Component.Route = "POEByEqTemplate";    
        }

        [Parameter]
        public List<PieceOfEquipment> LIST1 { get; set; }

        public override async Task<ResultSet<CalibrationSaaS.Domain.Aggregates.Entities.PieceOfEquipment>> LoadData(Pagination<CalibrationSaaS.Domain.Aggregates.Entities.PieceOfEquipment> pag)

        {
            Component.Route = "POEByEqTemplate";
            Component.IsModal = true;
            //ResultSet<Domain.Aggregates.Entities.PieceOfEquipment> r = new ResultSet<PieceOfEquipment>();
            Console.WriteLine("before filter");
            var order2 = pag.ColumnName.ToMemberOf<PieceOfEquipment>();
            var filterQuery = Querys.POEFilterNew(pag);
            Console.WriteLine("after filter");
            pag.Show = Grid.PageSize;

            List<PieceOfEquipment> res = new List<PieceOfEquipment>();

            if (pag.SortingAscending)
            {
                  res= LIST1.Where(filterQuery.Compile()).OrderBy(order2.Compile()).Take(pag.Top).ToList();
            }
            else
            {
                res= LIST1.Where(filterQuery.Compile()).OrderByDescending(order2.Compile()).Take(pag.Top).ToList();
            }
          

            //r.ClientPagination = true;
            //r.CurrentPage = 1;
            //r.Shown = 1000;
           
            //r.List = res;

            var t =QueryableExtensions2.GetTotalPages(res.Count,Grid.PageSize);
            pag.SaveCache = false;
            var countTotal = res.Count;
            res = res.Skip((pag.Page - 1) * Grid.PageSize).Take(Grid.PageSize).ToList();
            return new ResultSet<PieceOfEquipment>
            {
                CurrentPage = pag.Page,
                List = res,
                Count = countTotal,
                PageTotal = t,
                ColumnName = pag.ColumnName,
                SortingAscending = pag.SortingAscending,
                Shown=Grid.PageSize
            };
            
            
            //return r;

        }

    }
}
