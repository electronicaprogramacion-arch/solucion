using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Shared
{
    public partial class ModalWindow
    {

        [Inject] ILogger<ModalWindow> Logger { get; set; }

        protected ContentModal contenModal = new ContentModal();

        [Parameter]
        public string Type { get; set; }


#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        protected override async Task OnParametersSetAsync()
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            Init(Type);


        }


        public async Task CloseModalWindow()
        {
            //Logger.LogDebug("CloseModal");
            await JSRuntime.InvokeVoidAsync("closeModalWindow", contenModal.ID);
        }

        public void Init(string Type)
        {

            if (Type == "Customer")
            {

                contenModal.ID = "newCustomerModal";
                contenModal.HasCancelButton = true;
                contenModal.Title = "New Customer?";
                contenModal.CreateButons("Create", "btn btn-primary", "CreateCustomer");

                Card cr = new Card();
                cr.Category = "Customer Contact";
                cr.Title = "Certificate Address";
                cr.ImageUrl = "img/undraw_heavy_box_agqi.svg";

                Button b = new Button();
                b.Text = "Create";
                b.Class = "btn btn-success";
                b.Action = "Customer_Create";
                cr.Buttons.Add(b);

                contenModal.CreateContent(cr);

                cr = new Card();
                cr.Category = "Customer Contact";
                cr.Title = "Billing Address";
                cr.ImageUrl = "img/undraw_reviewed_docs_neeb.svg";
                cr.Buttons = new List<Button>();
                b = new Button();
                b.Text = "Create";
                b.Class = "btn btn-primary";
                b.Action = "CreateCustomer";
                cr.Buttons.Add(b);

                contenModal.CreateContent(cr);


            }
            else
            if (Type == "Assets")
            {

                contenModal.ID = "newAssetModal";
                contenModal.HasCancelButton = true;
                contenModal.Title = "New Asset?";
                Card crA = new Card();
                crA.Category = "Customer";
                crA.Title = "Piece of Equipment";
                crA.ImageUrl = "img/weight-balance-svgrepo-com.svg";
                crA.Buttons = new List<Button>();
                Button bA = new Button();
                bA.Text = "Create";
                bA.Class = "btn btn-success";
                bA.Action = "PieceOfEquipmentCreate";
                crA.Buttons.Add(bA);
                contenModal.CreateContent(crA);

                crA = new Card();
                crA.Category = "Company";
                crA.Title = "Standard";
                crA.ImageUrl = "img/control-system-svgrepo-com.svg";
                crA.CreateButons("Create", "btn btn-primary", "CreateStandard");
                contenModal.CreateContent(crA);

            }

        }

    }

    public class ContentModal
    {
        private int _CardsInRow = 2;
        public ContentModal()
        {
            Buttons = new List<Button>();
            Rows = new List<Row>();
        }
        public int CardsInRow
        {
            get
            {
                return _CardsInRow;
            }
            set
            {
                _CardsInRow = value;
            }
        }

        public string ID { get; set; }

        public string Title { get; set; }

        public List<Button> Buttons { get; set; }

        public List<Row> Rows { get; set; }

        public bool HasCancelButton { get; set; }


        public void CreateContent(Card item)
        {

            if (Rows.Count == 0 || Rows.Count % 2 == 0)
            {
                Row row = new Row();
                item.Border = "success";
                row.Cards.Add(item);
                Rows.Add(row);
            }
            else
           if (Rows.Count % CardsInRow != 0)
            {
                item.Border = "primary";
                Rows[Rows.Count - 1].Cards.Add(item);
            }
        }

        public void CreateButons(Button item)
        {
            Buttons.Add(item);
        }

        public void CreateButons(string _Text, string _Cssclass, string _Action)
        {
            Button bt = new Button()
            {
                Text = _Text,
                Class = _Cssclass,
                Action = _Action
            };
            Buttons.Add(bt);
        }




    }


    public class Row
    {
        public Row()
        {
            Cards = new List<Card>();
        }
        public List<Card> Cards { get; set; }


    }

    public class Card
    {
        public Card()
        {
            Buttons = new List<Button>();

        }

        public string Border { get; set; }

        public string Category { get; set; }

        public string Title { get; set; }

        public string ImageUrl { get; set; }

        public List<Button> Buttons { get; set; }

        public void CreateButons(Button item)
        {
            Buttons.Add(item);
        }

        public void CreateButons(string _Text, string _Cssclass, string _Action)
        {
            Button bt = new Button()
            {
                Text = _Text,
                Class = _Cssclass,
                Action = _Action
            };
            Buttons.Add(bt);
        }

    }


    public class Button
    {
        public string Class { get; set; }

        public string Text { get; set; }

        public string Action { get; set; }

    }
}
