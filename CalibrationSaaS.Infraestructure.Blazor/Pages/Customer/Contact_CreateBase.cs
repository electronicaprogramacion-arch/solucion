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

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Customer
{
    public class Contact_CreateBase
        : Base_Create<Contact, ICustomerService<CallContext>, Domain.Aggregates.Shared.AppStateCompany>
    //, IPage<Contact, ICustomerService<CallContext>, Domain.Aggregates.Shared.AppStateCompany>
    {


        public ResponsiveTable<Contact> Grid { get; set; }


        [Parameter]
        public List<Contact> List { get; set; } = new List<Contact>();


        public async Task<bool> Delete(Domain.Aggregates.Entities.Contact DTO)
        {


            await Client.DeleteContact(DTO, new CallContext());


            return true;


        }

        public Task SelectModal(Contact DTO)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Contact> FilterList(string filter = "")
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
