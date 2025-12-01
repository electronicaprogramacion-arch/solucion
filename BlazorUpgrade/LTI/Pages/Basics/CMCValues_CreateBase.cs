
using Blazed.Controls;
using Blazed.Controls.Route.Services;
using Blazed.Controls.Toast;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using CalibrationSaaS.Infraestructure.Blazor.Shared;

using Grpc.Core;
using Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CalibrationSaaS.Domain.Aggregates.Querys.Querys;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Basics
{
    public class CMCValues_CreateBase : Base_Create<CalibrationType, Application.Services.IBasicsServices<CallContext>, Domain.Aggregates.Shared.Basic.AppState>

    {
        public EditContext editContext { get; set; }

        public EditContext ec;

        
        protected ProgressBar eqtProgressBar;
        
        CMCValues _Group;

        public CMCValues CMCValueRef { get; set; } = new CMCValues();

        public CMCValues RowChange(CMCValues lin)
        {

            if (RT.IsDragAndDrop)
            {
                int posi = 100;
                var tmpg = RT.Items;
                for (int i = 0; i < RT.ItemList.Count; i++)
                {


                    RT.ItemList[i].CMCValueId = posi;
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

        public ResponsiveTable<CMCValues> RT { get; set; } = new ResponsiveTable<CMCValues>();


        public double MinRange { get; set; }
        public double MaxRange { get; set; }

        public List<CMCValues> LIST1 { get; set; } = new List<CMCValues>();
        
        public TestCode CurrentTestCode { get; set; }

        public Blazed.Controls.Typeahead<TestCode, TestCode> Typehead { get; set; }

        public CMCValues NewCMCValue()
        {

            return new CMCValues();


        }
        public void Show(CMCValues group)
        {

            if (group != null)
            {
                _Group = group;

                StateHasChanged();
            }

        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
           
            if (firstRender)
            {
                Load();
            }



        }
        public async Task FormSubmitted(EditContext e)
        {
            await ShowProgress();

           

                await JSRuntime.InvokeVoidAsync("showProgressBar", "progressBar");

                LastSubmitResult = "OnValidSubmit was executed";

                //
                if (RT?.Items?.Count > 0)
                {
                    eq.CMCValues = RT.Items;
                }


                BasicsServiceGRPC basics = new BasicsServiceGRPC(Client);
                var result = (await basics.InsertCMCValue(eq, new CallOptions()));

                await CloseModal(result);

                await ShowToast("The information has been saved successfully.", ToastLevel.Success);


            

        }

        public CMCValues Format()
        {

            return _Group;
        }
       
        //public async Task<TestCode> SearchTestCodeSelected(int? code)
        //{
        //    var result = TestCodeList.Where(x => x.TestCodeID == code);


        //    return result.FirstOrDefault();

        //}
        protected override async Task OnInitializedAsync()
        {
            try
            {
                
                NameValidationMessage = "valid";

                if (EntityID != "0")
                {
                    BasicsServiceGRPC basic = new BasicsServiceGRPC(Client);

                    eq.CalibrationTypeId = Convert.ToInt32(EntityID);

                    eq = await basic.GetCalibrationTypeById(eq);

                    if (eq.CMCValues != null)
                    {
                        LIST1 = eq.CMCValues.ToList();
                    }


                }
                else
                {
                    eq = new CalibrationType();
                   
                }

                await ScrollPosition();

            }
            catch (RpcException ex)
            {
                await ExceptionManager(ex);
            }

            catch (Exception ex)
            {
                await ExceptionManager(ex);
            }
            finally
            {
                LoadingWait = true;
                await CloseProgress();
                await OnInitializedAsync();
            }



        }
      
        public void Load()
        {
            
        }

    }
}
