
using Blazed.Controls.Toast;
using BlazorInputFile;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Basics
{
    public class Procedure_CreateBase : Base_Create<Procedure, Application.Services.IBasicsServices<CallContext>,
        Domain.Aggregates.Shared.Basic.AppState>
    {

#pragma warning disable CS0108 // 'Procedure_CreateBase.Logger' oculta el miembro heredado 'KavokuComponentBase<Procedure>.Logger'. Use la palabra clave new si su intención era ocultarlo.
        [Inject] ILogger<Procedure_CreateBase> Logger { get; set; }
#pragma warning restore CS0108 // 'Procedure_CreateBase.Logger' oculta el miembro heredado 'KavokuComponentBase<Procedure>.Logger'. Use la palabra clave new si su intención era ocultarlo.

#pragma warning disable CS0108 // 'Procedure_CreateBase.EnabledFunction()' oculta el miembro heredado 'Base_Create<Procedure, IBasicsServices<CallContext>, AppState>.EnabledFunction()'. Use la palabra clave new si su intención era ocultarlo.
        public Dictionary<string, object> EnabledFunction()
#pragma warning restore CS0108 // 'Procedure_CreateBase.EnabledFunction()' oculta el miembro heredado 'Base_Create<Procedure, IBasicsServices<CallContext>, AppState>.EnabledFunction()'. Use la palabra clave new si su intención era ocultarlo.
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
        public IFileListEntry file;
        [Inject] public IConfiguration Configuration { get; set; }
        [Inject] public CalibrationSaaS.Application.Services.IFileUpload fileUpload { get; set; }
        public string url { get; set; }
        public bool subscription { get; set; }
        public List<Domain.Aggregates.Entities.FileInfo> fileinfo = new List<Domain.Aggregates.Entities.FileInfo>();
        const string DefaultStatus = "Drop a csv file here to view it, or click to choose a CSV file";
        const int MaxFileSize = 5 * 1024 * 1024; // 5MB
        public string status = DefaultStatus;

        public string fileName="";
        public string fileTextContents;

        public async Task HandleFileSelected(IFileListEntry[] files)
        {
           var  _file = files.FirstOrDefault();
            file = _file;
            if (file == null) 
            {
                return;
            }
            fileName = file.Name;
             if (file.Size > MaxFileSize)
            {
                status = $"That's too big. Max size: {MaxFileSize} bytes.";
            }
           
        }
        protected override async Task OnParametersSetAsync()
        {

            //Logger.LogDebug(EntityID);

            NameValidationMessage = "valid";

            if (Convert.ToInt32(EntityID) == 0)
            {
                eq = new Procedure();
                //eq.IsEnabled = true;
            }
            else
            {



                if (AppState?.Procedures != null && AppState?.Procedures?.Count > 0)
                {
                    eq = AppState.Procedures.Where(x => x.ProcedureID == Convert.ToInt32(EntityID)).FirstOrDefault();
                    if (eq != null)
                    {

                        fileName = "Procedure_" + eq.ProcedureID.ToString();
                    }
                }
                else
                {
                    BasicsServiceGRPC b = new BasicsServiceGRPC(Client);

                    var all = await b.GetAllProcedures();

                    AppState.Procedures = all.Procedures;

                    eq = AppState.Procedures.Where(x => x.ProcedureID == Convert.ToInt32(EntityID)).FirstOrDefault();

                }





            }

            CurrentEditContext = new EditContext(eq);
        }

        [Parameter]
        public Func<Procedure, Task> EndExecute { get; set; }

        protected async Task FormSubmitted(EditContext editContext)
        {

            var validate = await ContextValidation(true);
            //if (file == null)
            //{
            //    await ShowToast("File is required ", ToastLevel.Error);
            //    return;
            //}

            if (validate)
            {
                Tenant tenant = new Tenant();
                //await ShowModal();

                LastSubmitResult = "OnValidSubmit was executed";


                try
                {


                    BasicsServiceGRPC basics = new BasicsServiceGRPC(Client);

                    ///
                    //
                    

                    var procedureExist = await basics.GetProcedureXName(eq, new CallContext());

                    if (procedureExist != null && procedureExist.Name != null)
                    {
                        await ShowError("Procedure Name already exists");
                        return;
                    }
                    ////

                    Result = (await basics.CreateProcedure(eq, new CallContext()));

                    if (file == null && (eq.DocumentUrl == "" || eq.DocumentUrl == null))
                    {
                        await ShowToast("A file or document URL is required.", ToastLevel.Error);
                        return;
                    }

                    if (file != null)
                    {
                        Domain.Aggregates.Entities.FileInfo f = new Domain.Aggregates.Entities.FileInfo();
                        f.Name = "Procedure_" + Result.ProcedureID + ".pdf";
                        f.Size = file.Size;
                        f.RelativePath = file.RelativePath;
                        f.LastModified = file.LastModified;
                        f.Type = file.Type;
                        //var stream = new MemoryStream();
                        //await file.Data.CopyToAsync(stream);

                        MemoryStream ms = new MemoryStream();
                        await file.Data.CopyToAsync(ms);
                        f.Data = ms.ToArray();
                        fileinfo.Add(f);
                        await fileUpload.UploadAsync(fileinfo.ToArray());
                    }

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
                    

                    await ShowError("Error Procedure Creation " + ex.Message);

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
            await base.OnInitializedAsync();

            TypeName = "Procedure";

            //CurrentEditContext.OnFieldChanged += HandleFieldChanged;
            url = Configuration.GetSection("Reports")["AzurePath"];

            subscription = Convert.ToBoolean(Configuration.GetSection("Reports")["EnableSubscription"]);

        }

        protected void FunctionName(EventArgs args)
        {
            if (eq.Name.Length < 1)
            {
                NameValidationMessage = "invalid";
            }
            else
            {
                NameValidationMessage = "valid";
            }
            //Logger.LogDebug(this.eq.Name);
            StateHasChanged();

        }

        public new void Dispose()
        {

        }

    }
}
