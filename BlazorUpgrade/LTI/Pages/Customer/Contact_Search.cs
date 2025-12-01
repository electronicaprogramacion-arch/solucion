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
using System.Linq;
using Blazed.Controls.Toast;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Customer
{
    public class Contact_SearchBase : Base_Create<Contact, Func<dynamic, ICustomerService<CallContext>>, Domain.Aggregates.Shared.AppStateCompany>
    {
        public BasicsServiceGRPC _basicsGrpc { get; set; }
        public CustomerGRPC _customerGrpc { get; set; }
        [Inject]
        IConfiguration Configuration { get; set; }

        [Parameter]
        public object CustomerFilterId { get; set; }

        public ResponsiveTable<Contact> Grid { get; set; } = new ResponsiveTable<Contact>();
        public List<Contact> ListContact = new List<Contact>();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        public override async Task<ResultSet<Contact>> LoadData(Pagination<Contact> pag)
        {
            Console.WriteLine($"Contact_Search LoadData - Connection Status: {CalibrationSaaS.Infraestructure.Blazor.Services.ConnectionStatusService.GetCurrentStatus()}");

            _customerGrpc = new CustomerGRPC(Client, DbFactory);

            string customerIdFilter = null;
            if (CustomerFilterId != null)
            {
                try
                {
                    if (CustomerFilterId is System.Text.Json.JsonElement jsonElement)
                    {
                        customerIdFilter = jsonElement.ToString();
                    }
                    else
                    {
                        customerIdFilter = CustomerFilterId.ToString();
                    }

                    Console.WriteLine($"Contact_Search: Filtering by CustomerID = {customerIdFilter}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error converting CustomerFilterId: {ex.Message}");
                    customerIdFilter = null;
                }
            }

            if (string.IsNullOrEmpty(customerIdFilter))
            {
                Console.WriteLine("Contact_Search: No CustomerFilterId provided, returning empty list");
                return new ResultSet<Contact> { List = new List<Contact>(), Count = 0 };
            }

            var customer = new CalibrationSaaS.Domain.Aggregates.Entities.Customer
            {
                CustomerID = customerIdFilter != null ? Convert.ToInt32(customerIdFilter) : 0
            };

            var result = await _customerGrpc.GetContactsByCustomID(customer, new CallContext());

            if (result is IEnumerable<Contact> enumerable)
            {
                var resultSet = new ResultSet<Contact>
                {
                    List = enumerable.ToList(),
                    Count = enumerable.Count()
                };
                ListContact = resultSet.List;
                return resultSet;
            }

            if (result is ResultSet<Contact> resultSetContact)
            {
                ListContact = resultSetContact.List;
                return resultSetContact;
            }

            var emptyResult = new ResultSet<Contact> { List = new List<Contact>(), Count = 0 };
            ListContact = emptyResult.List;
            return emptyResult;
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

            FormName = "Search Contact";
        }

        public void Dispose()
        {
            // Cleanup if needed
        }

        public async Task<bool> Delete(Contact DTO)
        {
            //_basicsGrpc = new BasicsServiceGRPC(Client, DbFactory);
            //await _basicsGrpc.DeleteContact(DTO, new CallOptions());
            return true;
        }

        public async Task SelectModal(Contact DTO)
        {
            await CloseModal(DTO);
        }

        protected async Task FormSubmitted(EditContext editContext)
        {
            CurrentEditContext = new EditContext(Grid.currentEdit);

            bool re = await ContextValidation(true);

            var msg = ValidationMessages;

            if (re)
            {
                try
                {
                    Result = null;
                    //_basicsGrpc = new BasicsServiceGRPC(Client, DbFactory);
                    //Result = (await _basicsGrpc.CreateContact(eq));


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
