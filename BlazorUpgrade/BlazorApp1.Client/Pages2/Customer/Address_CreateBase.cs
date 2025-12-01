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

namespace BlazorApp1.Blazor.Pages.Customer
{
    public class Address_CreateBase
        : Base_Create<Address, ICustomerService<CallContext>, CalibrationSaaS.Domain.Aggregates.Shared.AppStateCompany>
    //, IPage<Address, ICustomerService<CallContext>, Domain.Aggregates.Shared.AppStateCompany>
    {


        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

        }


        public ResponsiveTable<Address> RT { get; set; }

        [Parameter]
        public List<Address> List { get; set; } = new List<Address>();


        public async Task<bool> Delete(CalibrationSaaS.Domain.Aggregates.Entities.Address DTO)
        {


            await Client.DeleteAddress(DTO, new CallContext());


            return true;


        }

        public Task SelectModal(CalibrationSaaS.Domain.Aggregates.Entities.Address DTO)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CalibrationSaaS.Domain.Aggregates.Entities.Address> FilterList(string filter = "")
        {

            if (RT != null && RT.ItemsDataSource != null)
            {
                var templist = RT.ItemsDataSource;

                //return null;

                return templist.Where(i => i.StreetAddress1.ToLower().Contains(filter.ToLower()



                    )).ToArray();
            }
            else
            {
                return null;
            }


        }

    }
}
