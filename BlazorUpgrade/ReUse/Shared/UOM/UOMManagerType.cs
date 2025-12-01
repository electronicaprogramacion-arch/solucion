using Blazored.Modal;

using CalibrationSaaS.Application.Services;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Shared;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazed.Controls.Toast;

namespace CalibrationSaaS.Infraestructure.Blazor.Shared
{






    //    public class BaseService<T>: ComponentBase
    //{
    //    [Inject] public ICustomerService<CallContext> Client2 { get; set; }

    //    protected override async  Task OnInitializedAsync()
    //    {
    //        if (Client2 != null)
    //        {

    //        }

    //        await base.OnInitializedAsync();
    //    }


    //}





    public class UOMManagerType
        : Base_Create<UnitOfMeasureType, IUOMService<CallContext>, Domain.Aggregates.Shared.AppStateCompany>
    //, IPage<UnitOfMeasureType, CustomerGRPC, Domain.Aggregates.Shared.AppStateCompany>

    {
        //public new  Service  Client { get; set; }
        public ResponsiveTable<UnitOfMeasureType> Grid { get; set; } = new ResponsiveTable<UnitOfMeasureType>();
        [Parameter]
        public List<UnitOfMeasureType> List { get; set; } = new List<UnitOfMeasureType>();

       

        

        protected override Task OnParametersSetAsync()
        {

            return base.OnParametersSetAsync();
        }

        public ModalParameters Parameters = new ModalParameters();

        [Parameter]
        public bool IsNew { get; set; }

        public ModalParameters ParametersType = new ModalParameters();
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Parameters.Add("SelectOnly", true);


            //ParametersType.Add("IsNew", true);
            //CustomerGRPC p = new CustomerGRPC(Client);
            //var Service1 = new Service<CustomerGRPC>();
            //Customer cust = new Customer();
            //TenantDTO t = new TenantDTO();
            //t.TenantID = 23;
            ////await Service.GetCustomers(t, new CallOptions());
            //await p.GetCustomers(t);
            // await Client.GetService(p).GetCustomers(t);
            UOMServiceGRPC uom = new UOMServiceGRPC(Client);

            var lst = await uom.GetTypes();

            List = lst.ToList();

            //var types = await uom.GetTypes();

            //ListTypes = types.ToList();

        }

        public List<UnitOfMeasureType> ListTypes = new List<UnitOfMeasureType>();
        public Task SelectModal(UnitOfMeasureType DTO)
        {

            throw new NotImplementedException();
        }

        public IEnumerable<UnitOfMeasureType> FilterList(string filter = "")
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




        protected async Task FormSubmitted(EditContext editContext)
        {
            CurrentEditContext = new EditContext(Grid.currentEdit);

            bool re = await ContextValidation(true);

            var msg = ValidationMessages;
            //if (  re  || CustomValidation(eq))
            if (1 == 1)
            {

                try
                {

                    UOMServiceGRPC basics = new UOMServiceGRPC(Client);


                    Result = (await basics.CreateType(eq));


                    await ShowToast("Information Saved Successfully", ToastLevel.Success);

                    await CloseModal(Result);

                }
                catch (Exception ex)
                {
                    await ShowError("Error Save " + ex.Message);

                }

            }
        }


        protected async Task Submitted(ChangeEventArgs arg)
        {




            var aa = (UnitOfMeasureType)arg.Value;


            //CurrentEditContext = new EditContext(Grid.currentEdit);

            //bool re = await ContextValidation(true);


            //if (  re  || CustomValidation(eq))
            if (1 == 1)
            {


                try
                {

                    UOMServiceGRPC basics = new UOMServiceGRPC(Client);


                    Result = (await basics.CreateType(aa));


                    await ShowToast("Information Saved Successfully", ToastLevel.Success);


                    await CloseModal(Result);


                }

                catch (Exception ex)
                {


                    await ShowError("Error Save " + ex.Message);

                    throw ex;

                }

            }
        }

        protected async Task SubmittedUP(ChangeEventArgs arg)
        {

            var aa = (UnitOfMeasureType)arg.Value;


            //CurrentEditContext = new EditContext(Grid.currentEdit);

            //bool re = await ContextValidation(true);


            //if (  re  || CustomValidation(eq))
            if (1 == 1)
            {

                try
                {

                    UOMServiceGRPC basics = new UOMServiceGRPC(Client);


                    Result = (await basics.UpdateType(aa));


                    await ShowToast("Information Saved Successfully", ToastLevel.Success);

                    await CloseModal(Result);

                }
                catch (Exception ex)
                {
                    await ShowError("Error Save " + ex.Message);

                }

            }
        }
    }
}
