using Blazored.Modal;
using Blazored.Modal.Services;

using CalibrationSaaS.Application.Services;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Grpc.Core;
using Helpers.Controls.ValueObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazed.Controls.Toast;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Basics
{
    public class Procedure_SearchBase : Base_Create<Procedure, Application.Services.IBasicsServices<CallContext>, Domain.Aggregates.Shared.Basic.AppState>
    //ComponentBase,IPage<Procedure, Application.Services.IBasicsServices<CallContext>, AppState>
    {
        [CascadingParameter] BlazoredModalInstance BlazoredModal1 { get; set; }
        [CascadingParameter] public IModalService Modal { get; set; }




        public Search<Procedure, IBasicsServices<CallContext>, AppState> searchComponent
        { get; set; } = new Search<Procedure, IBasicsServices<CallContext>, AppState>();

        public ResponsiveTable<Procedure> Grid { get; set; } = new ResponsiveTable<Procedure>();
        public List<Procedure> ListMan = new List<Procedure>();

        //protected override async Task OnParametersSetAsync()
        //{


        //}

        public override async Task<ResultSet<Procedure>> LoadData(Pagination<Procedure> pag)
        {
            BasicsServiceGRPC basics = new BasicsServiceGRPC(Client);
            //Console.WriteLine("pag -- " + pag);

            var Eq = (await basics.GetProcedures(pag));

            //Console.WriteLine("pag -- " + pag);
            if (Eq.List != null)
            {
                //Console.WriteLine("Eq.List -- " + Eq);
                foreach (var item in Eq.List)
                {
                    var item2 = AppState.Procedures.Where(x => x.ProcedureID == item.ProcedureID).FirstOrDefault();

                    if (item2 == null)
                    {
                        AppState.AddProcedure(item);
                    }

                }
            }


            return Eq;

        }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }

            await base.OnParametersSetAsync();



            FormName = "Search Procedure";

            IsModal = IsModal;

            SelectOnly = SelectOnly;




        }



        public async Task<bool> Delete(Procedure DTO)
        {
            //await searchComponent.ShowModalAction();

            await Client.DeleteProcedure(DTO, new CallOptions());


            return true;

            //searchComponent.ShowResult();
        }

#pragma warning disable CS0108 
        public void Dispose()
        { 
#pragma warning restore CS0108 

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

                    BasicsServiceGRPC basics = new BasicsServiceGRPC(Client);


                    Result = (await basics.CreateProcedure(eq));


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




            var aa = (Procedure)arg.Value;


            //CurrentEditContext = new EditContext(Grid.currentEdit);

            //bool re = await ContextValidation(true);


            //if (  re  || CustomValidation(eq))
            if (1 == 1)
            {


                try
                {

                    BasicsServiceGRPC basics = new BasicsServiceGRPC(Client);


                    Result = (await basics.CreateProcedure(aa));


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

            var aa = (Procedure)arg.Value;



        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            


        }


    }
}
