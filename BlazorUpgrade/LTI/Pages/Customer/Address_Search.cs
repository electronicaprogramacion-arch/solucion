using CalibrationSaaS.Application.Services;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using CalibrationSaaS.Models.ViewModels;
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
    public class Address_SearchBase : Base_Create<CalibrationSaaS.Models.ViewModels.AddressCustomerViewModel, Func<dynamic, ICustomerService<CallContext>>, Domain.Aggregates.Shared.AppStateCompany>
    {
        public CustomerGRPC _customerGrpc { get; set; }

        [Inject]
        IConfiguration Configuration { get; set; }

        public ResponsiveTable<CalibrationSaaS.Models.ViewModels.AddressCustomerViewModel> Grid { get; set; } = new ResponsiveTable<CalibrationSaaS.Models.ViewModels.AddressCustomerViewModel>();
        public List<CalibrationSaaS.Models.ViewModels.AddressCustomerViewModel> ListAddress = new List<CalibrationSaaS.Models.ViewModels.AddressCustomerViewModel>();
        
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        public override async Task<ResultSet<CalibrationSaaS.Models.ViewModels.AddressCustomerViewModel>> LoadData(Pagination<CalibrationSaaS.Models.ViewModels.AddressCustomerViewModel> pag)
        {
            _customerGrpc = new CustomerGRPC(Client, DbFactory);

            // Use optimized method for better performance
            var Eq = (await _customerGrpc.GetAddressCustomerOptimized(pag));

            ListAddress = Eq.List;

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

            FormName = "Search Address";
        }

        public void Dispose()
        {
        }

        public async Task<bool> Delete(CalibrationSaaS.Models.ViewModels.AddressCustomerViewModel DTO)
        {
            CustomerGRPC cust = new CustomerGRPC(Client, DbFactory);
            // Convert ViewModel back to Address for deletion
            var address = DTO.ToAddress();
            await cust.DeleteAddress(address, new CallOptions());
            return true;
        }

        public async Task SelectModal(CalibrationSaaS.Models.ViewModels.AddressCustomerViewModel DTO)
        {
            await CloseModal(DTO);
        }

        protected async Task FormSubmitted(EditContext editContext)
        {
            CurrentEditContext = new EditContext(Grid.currentEdit);

            bool re = await ContextValidation(true);

            var msg = ValidationMessages;
            
            //if (1 == 1)
            //{
            //    try
            //    {
            //        CustomerGRPC basics = new CustomerGRPC(Client, DbFactory);
            //        Result = (await basics.CreateAddress(eq));

            //        await ShowToast("The information has been saved successfully.", ToastLevel.Success);
            //        await CloseModal(Result);
            //    }
            //    catch (Exception ex)
            //    {
            //        await ShowError("Error Save " + ex.Message);
            //    }
            //}
        }
    }
}
