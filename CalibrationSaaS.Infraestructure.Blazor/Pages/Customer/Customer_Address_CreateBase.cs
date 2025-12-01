using CalibrationSaaS.Infraestructure.Blazor.Shared;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Customer
{
    public class Customer_Address_CreateBase : ComponentBase
    {
        [Inject] Application.Services.ICustomerAddressService Client { get; set; }

        protected Domain.Aggregates.Entities.CustomerAddress customerAddress = new Domain.Aggregates.Entities.CustomerAddress();

        //demo
        [Inject] System.Net.Http.HttpClient Http { get; set; }

        protected StandardModal child;
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
