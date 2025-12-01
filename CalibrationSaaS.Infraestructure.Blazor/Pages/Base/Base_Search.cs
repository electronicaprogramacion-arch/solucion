using Blazored.Modal;
using Blazored.Modal.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Infraestructure.Blazor.Shared;
using CalibrationSaaS.Infraestructure.Blazor.Shared.Component;
using Helpers.Controls.ValueObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Base
{
    public class Base_Search<T, S, D> : ComponentBase where T : IGeneric, new()
    {


        //[Inject] ILogger Logger { get; set; }
        public int paginaActual { get; set; } = 1;
        public int paginasTotales { get; set; } = 10;
        public int paginaSize { get; set; } = 10;
        public int TotalRows { get; set; } = 0;

        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; }
        [CascadingParameter] public IModalService Modal { get; set; }

        protected ProgressBar progressBar;

        public StandardModal child;

        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] System.Net.Http.HttpClient Http { get; set; }

        [Inject] public S Client { get; set; }

        [Inject] public D AppState { get; set; }

        [Parameter]
        public Func<string, IEnumerable<T>> FuncFilter { get; set; }


        public bool SelectOnly { get; set; }
        public bool IsModal { get; set; }



        public T Detail { get; set; } = new T();

        public string BaseCreateUrl { get; set; }
        public string BaseDetailUrl { get; set; }

        public string SearchTerm { get; set; } = "";

        #region Page
        [Parameter]
        public int PageSize { get; set; }


        public string TypeName { get; set; }


        public int totalPages;
        public int curPage;
        public int pagerSize;

        public int startPage;
        public int endPage;

        public string b = "back";

        public string p = "previous";


        public void SetPagerSize(string direction)
        {
            if (direction == "forward" && endPage < totalPages)
            {
                startPage = endPage + 1;
                if (endPage + pagerSize < totalPages)
                {
                    endPage = startPage + pagerSize - 1;
                }
                else
                {
                    endPage = totalPages;
                }
                //this.StateHasChanged();
            }
            else if (direction == "back" && startPage > 1)
            {
                endPage = startPage - 1;
                startPage = startPage - pagerSize;
            }
        }

        public async Task NavigateToPage(string direction)
        {
            await this.ShowProgress();

            if (direction == "next")
            {
                if (curPage < totalPages)
                {
                    if (curPage == endPage)
                    {
                        SetPagerSize("forward");
                    }
                    curPage += 1;
                }
            }
            else if (direction == "previous")
            {
                if (curPage > 1)
                {
                    if (curPage == startPage)
                    {
                        SetPagerSize("back");
                    }
                    curPage -= 1;
                }
            }

            await updateList(curPage);
        }

        public async Task updateList(int currentPage)
        {
            if (List == null)
            {
                return;
            }
            await this.ShowProgress();
            FilteredToDos = List.Skip((currentPage - 1) * PageSize).Take(PageSize);
            curPage = currentPage;
            totalPages = (int)Math.Ceiling(List.Count() / (decimal)PageSize);
            SetPagerSize("forward");
            //this.StateHasChanged();
            await this.CloseProgress();
        }
        #endregion

        public string ModalName { get; set; } = "detailEquipment";

        public object Format(object value)
        {

            return value ??= "";
        }

        IEnumerable<T> _FilteredToDos = new List<T>();


        public IEnumerable<T> FilteredToDos
        {

            get
            {
                return FilterList();
            }
            set
            {

                _FilteredToDos = value;


            }
        }


        public DetailView<T> ModalDetail = new DetailView<T>();


        public string FormName { get; set; } = "";


        public List<T> List = new List<T>();
        //public List<T> FilteredToDos => List.Where(i => i.Name.ToLower().Contains(SearchTerm.ToLower())).ToList();
        public string CreateUrl(string ID)
        {
            return BaseCreateUrl + "/" + ID;

        }

        public string DeletUrl(string ID)
        {
            return BaseCreateUrl + "/" + ID;

        }

        public virtual IEnumerable<T> FilterList()
        {
            //Logger.LogDebug("FilterList");
            if (FuncFilter != null)
            {
                // Logger.LogDebug("1-FilterList");

                var result = FuncFilter(SearchTerm);

                if (result == null)
                {
                    return null;
                }

                var ItemList = result.Skip((curPage - 1) * PageSize).Take(PageSize);
                totalPages = (int)Math.Ceiling(result.Count() / (decimal)PageSize);

                SetPagerSize("forward");


                return ItemList;
            }
            else
            {
                // Logger.LogDebug("2-FilterList");
                return null; // List.Where(i => i.Name.ToLower().Contains(SearchTerm.ToLower())).ToList();
            }


            //return List.Where(i => i.Name.ToLower().Contains(SearchTerm.ToLower())).ToList();

        }



        public async Task ShowModal()
        {
            await JSRuntime.InvokeVoidAsync("showModal", ModalName);
            StateHasChanged();
        }

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public virtual async Task GetDetail(int ID)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {


        }



        public void KeyEvent(string FieldName, object Field)
        {


        }

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        protected override async Task OnInitializedAsync()
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {


        }
        protected override Task OnParametersSetAsync()
        {
            return base.OnParametersSetAsync();
        }


        //public async Task ShowModalAction()
        //{
        //    await child.ShowModal("We are creating: " + Detail.Name, "Please wait", true, true, "Add more " + TypeName);

        //}

        //public void ShowResult()
        //{

        //    child.ShowResult("The " + TypeName + "  was Delete: " , TypeName + " Name: " + Detail.Name, "", "");

        //}


        public async Task SelectModal(T item)
        {
            await BlazoredModal.CloseAsync(ModalResult.Ok(item));
        }

        //protected override async Task OnInitializedAsync()
        //{
        //    //BaseCreateUrl = "ManufacturerCreate";

        //    //BaseDetailUrl = "ManufacturerDetail";

        //    //var Eq = (await Client.GetManufacturers());

        //    //List = Eq.Manufacturers;

        //    //Console.Write("OnInitializedAsync");

        //    //foreach (var item in ManufacturerList)
        //    //{
        //    //    AppState.AddManufacturer(item);
        //    //}

        //}





        public void Dispose()
        {

        }

        public async Task ShowProgress()
        {
            await this.progressBar.ShowProgressBar();
        }

        public async Task CloseProgress()
        {
            await this.progressBar.CloseProgressBar();
        }


#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        public virtual async Task<ResultSet<PieceOfEquipment>> LoadData(Pagination<PieceOfEquipment> pag)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {

            throw new NotImplementedException();

        }

    }
}
