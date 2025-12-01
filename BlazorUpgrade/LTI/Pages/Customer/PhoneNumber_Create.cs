using CalibrationSaaS.Application.Services;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Shared;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Grpc.Core;
using Microsoft.AspNetCore.Components;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Customer
{
    public class PhoneNumbers_CreateBase
        : Base_Create<PhoneNumber, Func<dynamic, Application.Services.ICustomerService<CallContext>>, Domain.Aggregates.Shared.AppStateCompany>

    {


        public ResponsiveTable<PhoneNumber> Grid { get; set; }


        [Parameter]
        public List<PhoneNumber> List { get; set; } = new List<PhoneNumber>();



        public async Task<bool> Delete(Domain.Aggregates.Entities.PhoneNumber DTO)
        {
            try
            {
               

                //await searchComponent.ShowModalAction();
                CustomerGRPC basic = new CustomerGRPC(Client,DbFactory);

                var result = await basic.DeletePhone(DTO);



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


        public Task SelectModal(PhoneNumber DTO)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PhoneNumber> FilterList(string filter = "")
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
