
using CalibrationSaaS.Application.Services;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Grpc.Core;
using Helpers.Controls.ValueObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Blazed.Controls.Toast;
using Microsoft.Extensions.Configuration;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Customer
{
    public class Customer_SearchBase : Base_Create<CalibrationSaaS.Domain.Aggregates.Entities.Customer, Func<dynamic, ICustomerService<CallContext>>, Domain.Aggregates.Shared.AppStateCompany>
    //ComponentBase, 
    //IPage<CalibrationSaaS.Domain.Aggregates.Entities.Customer, ICustomerService<CallContext>, AppStateCompany>
    {
        //public Search<Domain.Aggregates.Entities.Customer, ICustomerService<CallContext>, AppStateCompany> searchComponent { get ; set ; } = new Search<Domain.Aggregates.Entities.Customer, ICustomerService<CallContext>, AppStateCompany>();
       
        public CustomerGRPC _customerGrpc { get; set; }
       
        
        [Inject]
        IConfiguration Configuration { get; set; }

        public bool showEbms { get; set; } = false;

        public ResponsiveTable<Domain.Aggregates.Entities.Customer> Grid { get; set; } = new ResponsiveTable<Domain.Aggregates.Entities.Customer>();
        public List<Domain.Aggregates.Entities.Customer> ListCustomer = new List<Domain.Aggregates.Entities.Customer>();
        protected override async Task OnInitializedAsync()
        {
            

            await base.OnInitializedAsync();

            string customer = Configuration.GetSection("Reports")["Customer"];

            if (customer == "Bitterman")
            {
                showEbms = true;
            }
            else
            {
                showEbms = false;
            }


        }


        //public IEnumerable<Domain.Aggregates.Entities.Customer> FilterList(string filter)
        //{
        //    if(searchComponent.List == null)
        //    {
        //        return null;
        //    }

        //     return searchComponent.List.Where(i => i.Name.ToLower().Contains(filter.ToLower()) 
        //     || i.CustomerID.ToString().ToLower().Contains(filter) 
        //     || i.Description.ToString().ToLower().Contains(filter)).ToArray();
        //}


        public override async Task<ResultSet<Domain.Aggregates.Entities.Customer>> LoadData(Pagination<Domain.Aggregates.Entities.Customer> pag)
        {
            //return base.LoadData(pag);

            _customerGrpc = new CustomerGRPC(Client,DbFactory);
            var Eq = (await _customerGrpc.GetCustomers(pag));
            //if (Eq != null)
            //{

            //searchComponent.List = Eq.Customers;

            //searchComponent.FilteredToDos = Eq.Customers;

            ListCustomer = Eq.List;

            //StateHasChanged();



            //}
            return Eq;

        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }

            await base.OnParametersSetAsync();
            SelectOnly = SelectOnly;

            IsModal = IsModal;

            //BaseCreateUrl = "Customer";

            //BaseDetailUrl = "CustomerDetail";

            FormName = "Search Customer";

            //ModalName = "detailView";

            Tenant tenant = new Tenant();

            //if(searchComponent.Client != null) 
            //{
            //var Eq = (await searchComponent.Client.GetCustomers());
            //_customerGrpc = new CustomerGRPC(Client);
            //var Eq = (await _customerGrpc.GetCustomers(new CallOptions()));
            //if (Eq != null)
            //{

            //    searchComponent.List = Eq.Customers;

            //    searchComponent.FilteredToDos= Eq.Customers;

            //    ListCustomer = Eq.Customers;

            //    StateHasChanged();
            //}
            //}
            //else
            //{
            //  //  Logger.LogDebug("cliente nulloooo");
            //}

        }



#pragma warning disable CS0108 // 'Customer_SearchBase.Dispose()' oculta el miembro heredado 'Base_Create<Customer, ICustomerService<CallContext>, AppStateCompany>.Dispose()'. Use la palabra clave new si su intención era ocultarlo.
        public void Dispose()
#pragma warning restore CS0108 // 'Customer_SearchBase.Dispose()' oculta el miembro heredado 'Base_Create<Customer, ICustomerService<CallContext>, AppStateCompany>.Dispose()'. Use la palabra clave new si su intención era ocultarlo.
        {

        }




        public async Task<bool> Delete(Domain.Aggregates.Entities.Customer DTO)
        {
            //await searchComponent.ShowModalAction();

            CustomerGRPC cust = new CustomerGRPC(Client,DbFactory);

            await cust.DeleteCustomer(DTO, new CallOptions());


            return true;

            //searchComponent.ShowResult();
        }
#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public async Task SelectModal(Domain.Aggregates.Entities.Customer DTO)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            throw new NotImplementedException();
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

                    CustomerGRPC basics = new CustomerGRPC(Client,DbFactory);


                    Result = (await basics.CreateCustomer(eq));


                    await ShowToast("The information has been saved successfully.", ToastLevel.Success);

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
