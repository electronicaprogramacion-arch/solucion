using Blazored.Modal;
using Blazored.Modal.Services;

using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using CalibrationSaaS.Infraestructure.Blazor.Shared.Component;
using Grpc.Core;
using Helpers.Controls.ValueObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blazed.Controls.Toast;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Company
{
    public class User_SearchBase : Base_Create<User, Application.Services.IBasicsServices<CallContext>, Domain.Aggregates.Shared.AppStateCompany>
    //ComponentBase, IPage<User, Application.Services.ICompanyServices, CalibrationSaaS.Domain.Aggregates.Shared.AppStateCompany>
    {


        [CascadingParameter] BlazoredModalInstance BlazoredModal1 { get; set; }
        [CascadingParameter] public IModalService Modal { get; set; }

      

        public BasicsServiceGRPC _basicsGrpc { get; set; }
        //public Search<User, Application.Services.ICompanyServices,
        //CalibrationSaaS.Domain.Aggregates.Shared.AppStateCompany> searchComponent
        //{ get; set; } = new Search<User, Application.Services.ICompanyServices, Domain.Aggregates.Shared.AppStateCompany>();

        [Inject] public Application.Services.IBasicsServices<CallContext> _basicsServices { get; set; }

        public ResponsiveTable<User> Grid { get; set; } = new ResponsiveTable<User>();
        public List<User> ListUser = new List<User>();


        public override async Task<ResultSet<User>> LoadData(Pagination<User> pag)
        {
            Loading=true;
            //BasicsServiceGRPC basics = new BasicsServiceGRPC(Client);

            //  var Eq = (await basics.GetUsers());

            ////////////////////////////////////////////////
            BasicsServiceGRPC basics = new BasicsServiceGRPC(Client);

            //var Eq = await basics.GetEquipment();//(await searchComponent.Client.GetEquipment());
            // pag.ColumnName = "EquipmentTypeID";
            var Eq = (await basics.GetUsersPag(pag));


            Loading = false;
            return Eq;



        }

        public async Task<bool> Delete(User DTO)
        {
            //await searchComponent.ShowModalAction();
            try
            {
                await Client.DeleteUser(DTO, new CallOptions());

                await ShowToast("Delete sucessful",ToastLevel.Success);

                return true;
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


            //searchComponent.ShowResult();
        }


        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            //BaseCreateUrl = "UserCreate";

            //BaseDetailUrl = "UserDetail";


            return;

            FormName = "Search User";



            IsModal = IsModal;

            SelectOnly = SelectOnly;


            if (Client != null)
            {

                BasicsServiceGRPC basics = new BasicsServiceGRPC(Client);

                var Eq = (await basics.GetUsers());

                //searchComponent.List = Eq?.Users;

                ////foreach (var item in searchComponent.List)
                ////{
                ////    searchComponent.AppState.AddUser(item);
                ////}

                //searchComponent.FilteredToDos = Eq?.Users;
                ListUser = Eq.Users;

                StateHasChanged();
            }
        }



#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        protected override async Task OnAfterRenderAsync(bool firstRender)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            if (!firstRender)
            {
                return;
            }


        }

        //public IEnumerable<User> FilterList(string filter)
        //{
        //    if(searchComponent != null && searchComponent.List != null && searchComponent.List.Count> 0)
        //    {
        //        return searchComponent.List.Where(i => i.Name.ToLower().Contains(filter.ToLower())).ToList();

        //    }
        //    return null;


        //}
        //public async Task SelectModal(User DTO)
        //{
        //    await BlazoredModal1.Close(ModalResult.Ok(searchComponent.Detail));
        //}





        //public  async Task GetDetail(User DTO)
        //{
        //    searchComponent.Detail = DTO; // searchComponent.FilteredToDos.Where(x => x.UserID == ID).FirstOrDefault();

        //    await ModalDetail.ShowModal(searchComponent.Detail);
        //    //await ShowModal();

        //    //StateHasChanged();
        //}

        public DetailView<User> ModalDetail = new DetailView<User>();



#pragma warning disable CS0108 // 'User_SearchBase.Dispose()' oculta el miembro heredado 'Base_Create<User, IBasicsServices<CallContext>, AppStateCompany>.Dispose()'. Use la palabra clave new si su intención era ocultarlo.
        public void Dispose()
#pragma warning restore CS0108 // 'User_SearchBase.Dispose()' oculta el miembro heredado 'Base_Create<User, IBasicsServices<CallContext>, AppStateCompany>.Dispose()'. Use la palabra clave new si su intención era ocultarlo.
        {

        }

        protected async Task FormSubmitted(EditContext editContext)
        {

            return;
            
            CurrentEditContext = new EditContext(Grid.currentEdit);

            //bool re = await ContextValidation(true);

            var msg = ValidationMessages;
            //if (  re  || CustomValidation(eq))
            if (1 == 1)
            {

                try
                {

                    BasicsServiceGRPC basics = new BasicsServiceGRPC(Client);


                    Result = (await basics.CreateUser(eq));


                    await ShowToast("The information has been saved successfully.", ToastLevel.Success);

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


            return;

            var aa = (User)arg.Value;


            //CurrentEditContext = new EditContext(Grid.currentEdit);

            //bool re = await ContextValidation(true);


            //if (  re  || CustomValidation(eq))
            if (1 == 1)
            {


                try
                {

                    BasicsServiceGRPC basics = new BasicsServiceGRPC(Client);


                    Result = (await basics.CreateUser(aa));


                    await ShowToast("The information has been saved successfully.", ToastLevel.Success);


                    await CloseModal(Result);


                }

                catch (Exception ex)
                {


                    await ShowError("Error Save " + ex.Message);

                    throw ex;

                }

            }
        }

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        protected async Task SubmittedUP(ChangeEventArgs arg)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {

            var aa = (User)arg.Value;



        }


        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();




        }

    }
}
