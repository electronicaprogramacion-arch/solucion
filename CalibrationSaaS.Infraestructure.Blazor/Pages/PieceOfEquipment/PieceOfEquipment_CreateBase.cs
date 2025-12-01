using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using CalibrationSaaS.Infraestructure.Blazor.Shared;
using System.Net.Http.Json;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.PieceOfEquipment
{
    public class PieceOfEquipment_CreateBase : ComponentBase
    {
        [Inject] Application.Services.IPieceOfEquipmentService Client { get; set; }

        protected Domain.Aggregates.Entities.PieceOfEquipment pieceOfEquipment = new Domain.Aggregates.Entities.PieceOfEquipment();

        //demo
        [Inject] System.Net.Http.HttpClient Http {get;set;}

        protected StandardModal child;
        protected async Task CreatePieceOfEquipment()
        {
            //await child.ShowModal("We are creating: " + this.pieceOfEquipment.SerialNumber, "Please wait", true, true, "Add more Pieces of Equipment");
            
            this.pieceOfEquipment.TenantId = 1;
            
            var result = await Client.CreatePieceOfEquipment(this.pieceOfEquipment);
            //just for demo
            //await AddItem(this.customer.CustomerName);

            child.ShowResult("The piece of equipment was created: " + this.pieceOfEquipment.PieceOfEquipmentId, "Piece Of Equipment Id: " + result.PieceOfEquipmentId, "PieceOfEquipment/" + result.PieceOfEquipmentId, "Create Piece of Equipment");
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
