

using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BlazorApp1.Blazor.Pages.Customer
{
    public class Customer_Address_CreateBase : ComponentBase
    {
        [Inject] CalibrationSaaS.Application.Services.ICustomerAddressService Client { get; set; }

        protected CalibrationSaaS.Domain.Aggregates.Entities.CustomerAddress customerAddress = new CalibrationSaaS.Domain.Aggregates.Entities.CustomerAddress();

        //demo
        [Inject] System.Net.Http.HttpClient Http { get; set; }

        //protected StandardModal child;
        protected async Task CreateCustomerAddress()
        {
            var result = await Client.CreateCustomerAddress(this.customerAddress);
            //just for demo
            //await AddItem(this.customer.CustomerName);

            child.ShowResult("The customer was created: " + this.customerAddress.CustomerAddressId, "Customer Address Id: " + result.CustomerAddressId, "OrderCreate/" + result.CustomerAddressId, "Create Work Order");
        }

        private async Task AddItem(string name)
        {
            var addItem = new { name = name };
            var content = new System.Net.Http.FormUrlEncodedContent(new[]
            {
                new System.Collections.Generic.KeyValuePair<string, string>("name", name)
            });
            Http.BaseAddress = new System.Uri("https://localhost:44340");
            await Http.PostAsync("/Home/AddCustomerAddress", content);

        }
    }
}
