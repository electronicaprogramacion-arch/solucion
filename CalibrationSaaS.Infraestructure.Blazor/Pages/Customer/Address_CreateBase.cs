using CalibrationSaaS.Application.Services;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Shared;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Customer
{
    public class Address_CreateBase
        : Base_Create<Address, ICustomerService<CallContext>, Domain.Aggregates.Shared.AppStateCompany>
    //, IPage<Address, ICustomerService<CallContext>, Domain.Aggregates.Shared.AppStateCompany>
    {
        [Inject] protected IConfiguration Configuration { get; set; }
        [Inject] protected ILogger<Address_CreateBase> Logger { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

        }


        public ResponsiveTable<Address> RT { get; set; }

        [Parameter]
        public List<Address> List { get; set; } = new List<Address>();


        public async Task<bool> Delete(Domain.Aggregates.Entities.Address DTO)
        {


            await Client.DeleteAddress(DTO, new CallContext());


            return true;


        }

        public Task SelectModal(Domain.Aggregates.Entities.Address DTO)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Domain.Aggregates.Entities.Address> FilterList(string filter = "")
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

        protected bool IsThermotempCustomer()
        {
            try
            {
                string customer = Configuration.GetSection("Reports")?["Customer"];
                return customer == "Thermotemp";
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, "Error checking Thermotemp customer configuration");
                return false;
            }
        }

    }
}
