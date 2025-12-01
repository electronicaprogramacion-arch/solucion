
using Blazed.Controls;
using Blazed.Controls.Toast;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Grpc.Core;
using Helpers.Controls.ValueObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CalibrationSaaS.Domain.Aggregates.Querys.Querys;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Basics
{
    public class TestCode_CreateBase : Base_Create<TestCode, Application.Services.IWorkOrderDetailServices<CallContext>,
        Domain.Aggregates.Shared.Basic.AppState>
    {

        //[Parameter]
        //public bool Enabled2 { get; set; } = true;
        [Inject] Microsoft.Extensions.Configuration.IConfiguration Configuration { get; set; }
        [Inject] Application.Services.IWorkOrderDetailServices<CallContext> _wod { get; set; }
        public List<CalibrationSubType> _calibrationSubtypeList = new List<CalibrationSubType>();
        [Inject] public Application.Services.IBasicsServices<CallContext> _basicsServices { get; set; }
#pragma warning disable CS0108 // 'Manufacturer_CreateBase.Logger' oculta el miembro heredado 'KavokuComponentBase<Manufacturer>.Logger'. Use la palabra clave new si su intención era ocultarlo.
        [Inject] ILogger<TestCode_CreateBase> Logger { get; set; }
#pragma warning restore CS0108 // 'Manufacturer_CreateBase.Logger' oculta el miembro heredado 'KavokuComponentBase<Manufacturer>.Logger'. Use la palabra clave new si su intención era ocultarlo.

#pragma warning disable CS0108 // 'Manufacturer_CreateBase.EnabledFunction()' oculta el miembro heredado 'Base_Create<Manufacturer, IBasicsServices<CallContext>, AppState>.EnabledFunction()'. Use la palabra clave new si su intención era ocultarlo.
        public Dictionary<string, object> EnabledFunction()
#pragma warning restore CS0108 // 'Manufacturer_CreateBase.EnabledFunction()' oculta el miembro heredado 'Base_Create<Manufacturer, IBasicsServices<CallContext>, AppState>.EnabledFunction()'. Use la palabra clave new si su intención era ocultarlo.
        {
            if (!Enabled)
            {
                var dict = new Dictionary<string, object>();
                dict.Add("disabled", "disabled");
                return dict;
            }
            else
            {
                return null;
            }

        }
        public string Customer { get; set; }
        public List<Procedure> procedureList_ = new List<Procedure>();

        //[Parameter]        
        //public new bool  Enabled { get; set; }
        protected override async Task OnParametersSetAsync()
        {

           
        }

        public ResponsiveTable<Note> RT { get; set; } = new ResponsiveTable<Note>();
        public Note NoteRef { get; set; } = new Note();
        public List<Note> LIST1 { get; set; } = new List<Note>();
        public Note NewNote()
        {

            return new Note();


        }


        public Note RowChange(Note lin)
        {



            if (RT.IsDragAndDrop)
            {
                int posi = 100;
                
                for (int i = 0; i < RT.ItemList.Count; i++)
                {
                    RT.ItemList[i].Position = posi;
                    posi++;
                    RT.ReplaceItemKey(RT.ItemList[i]);
                }

                return lin;
            }

            if (lin.Position >= 100)
            {
                return lin;
            }

            return lin;



        }






        [Parameter]
        public Func<TestCode, Task> EndExecute { get; set; }

        protected async Task FormSubmitted(EditContext editContext)
        {

            var validate = await ContextValidation(true);

            if (validate || CustomValidation(eq))
            {
                Tenant tenant = new Tenant();
                //await ShowModal();

                LastSubmitResult = "OnValidSubmit was executed";


                try
                {


                    if (RT?.Items?.Count > 0)
                    {
                        eq.Notes = RT.Items;
                    }

                    var tc = AppSecurity.EquipmentTypeWODList.Where(x => x.EquipmentTypeID == eq.EquipmentTypeID).FirstOrDefault();

                    eq.CalibrationTypeID= tc?.CalibrationTypeID;    

                    WorkOrderDetailGrpc basics = new WorkOrderDetailGrpc(Client);
                    ///
                    //


                    var testCodeExist = await basics.GetTestCodeXName(eq, new CallContext());

                    if (testCodeExist != null && testCodeExist.Code != null)
                    {
                        await ShowError("TestCode Code already exists");
                        return;
                    }
                    ////


                    Result = (await basics.CreateTestCode(eq, new CallContext()));

                    await ShowToast("The information has been saved successfully.", ToastLevel.Success);

                    await CloseModal(Result);

                    if (EndExecute != null)
                    {
                        await EndExecute(Result);
                    }

                    basics.Dispose();

                }

                catch (Exception ex)

                {
                    

                    await ShowError("Error Test Code Creation " + ex.Message);

                }


                //ShowResult();
            }

        }
#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        protected async Task InvalidFormSubmitted(EditContext editContext)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            LastSubmitResult = "OnInvalidSubmit was executed";
        }


        protected override async Task OnInitializedAsync()
        {

            Customer = Configuration.GetSection("Reports")["Customer"];
            
            

            await base.OnInitializedAsync();

            TypeName = "Test Code";
            
            //CurrentEditContext.OnFieldChanged += HandleFieldChanged;
           var procedureList = await _basicsServices.GetAllProcedures(new CallOptions());
            
            if (procedureList?.Procedures?.Count() > 0)
            {
                procedureList_ = procedureList.Procedures;
            }

             //Logger.LogDebug(EntityID);

            NameValidationMessage = "valid";

            if (Convert.ToInt32(EntityID) == 0)
            {
                eq = new TestCode();
                eq.Accredited = true;

            }
            else
            {               
                    WorkOrderDetailGrpc b = new WorkOrderDetailGrpc(Client);

                    Pagination<TestCode> p = new Pagination<TestCode>();

                p.Show = 1000;
                p.Page = 1;


                TestCode item = new TestCode();
                item.TestCodeID = Convert.ToInt32(EntityID);

                    eq = await b.GetTestCodeByID(item);

                if (eq.TestCodeID == 0)
                {
                    eq.Accredited = true;
                }
                // eq = all.List.Where(x => x.TestCodeID == Convert.ToInt32(EntityID)).FirstOrDefault();
                if (eq.Notes != null)
                {
                    LIST1 = eq.Notes.ToList();
                }


            }
            var dataGrids = await _wod.GetCalibrationSubtype(new CallContext());


            _calibrationSubtypeList = dataGrids;

            CurrentEditContext = new EditContext(eq);



            
        }

       

        public new void Dispose()
        {

        }

    }
}
