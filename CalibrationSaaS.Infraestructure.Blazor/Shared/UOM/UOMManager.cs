using Blazored.Modal;

using CalibrationSaaS.Application.Services;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Domain.Aggregates.Shared;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Base;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
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

namespace CalibrationSaaS.Infraestructure.Blazor.Shared
{





    public interface IService<T>
    {

        public S GetService<S>(S service) where S : IProp<T>;


    }



    //    public class BaseService<T>: ComponentBase
    //{
    //    [Inject] public ICustomerService<CallContext> Client2 { get; set; }

    //    protected override async  Task OnInitializedAsync()
    //    {
    //        if (Client2 != null)
    //        {

    //        }

    //        await base.OnInitializedAsync();
    //    }


    //}

    public interface IProp<T>
    {
        public T Client2 { get; set; }

    }


    public class Service<T> : IService<T>
    {
        [Inject] public T Client2 { get; set; }



        public S GetService<S>(S val) where S : IProp<T>
        {

            //val.Client2 = Client2;

            return val;
        }


    }







    public class UOMManager
        : Base_Create<UnitOfMeasure, IUOMService<CallContext>, Domain.Aggregates.Shared.AppStateCompany>
    //, IPage<UnitOfMeasure, CustomerGRPC, Domain.Aggregates.Shared.AppStateCompany>

    {
        //public new  Service  Client { get; set; }
        public ResponsiveTable<UnitOfMeasure> Grid { get; set; } = new ResponsiveTable<UnitOfMeasure>();
        [Parameter]
        public List<UnitOfMeasure> List { get; set; } = new List<UnitOfMeasure>();

       

       

        public async Task<bool> Delete(Domain.Aggregates.Entities.UnitOfMeasure DTO)
        {
            try
            {
                UOMServiceGRPC uom = new UOMServiceGRPC(Client);

                await uom.Delete(DTO);

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

        }

        protected override Task OnParametersSetAsync()
        {

            return base.OnParametersSetAsync();
        }

        public ModalParameters Parameters = new ModalParameters();

        public ModalParameters ParametersType = new ModalParameters();

        public UnitOfMeasure DefaultItem()
        {
            UnitOfMeasure um = new UnitOfMeasure();

            um.IsEnabled = true;

            return um;

        }

        [Parameter]
        public UnitOfMeasure Filter { get; set; }

        //UnitOfMeasure unitOfMeasure = new UnitOfMeasure();
        public string Bound(UnitOfMeasure u)
        {
            Parameters = new ModalParameters();

            Parameters.Add("SelectOnly", true);

            Parameters.Add("Filter", u);

            eq = u;

            return "";

        }




        public override async Task<ResultSet<UnitOfMeasure>> LoadData(Pagination<UnitOfMeasure> pag)
        {
           

            if (Filter != null && Filter?.TypeID > 0)
            {
                var a = Filter.TypeID.GetUoMType(AppState.UnitofMeasureTypeList);

                pag.Filter = a.Name;
            }

            UOMServiceGRPC uom = new UOMServiceGRPC(Client);

            var lst = await uom.GetAllPag(pag);

            //List = lst.UnitOfMeasureList;

            return lst;

        }


        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

           

            Parameters.Add("SelectOnly", true);

            UOMServiceGRPC uom = new UOMServiceGRPC(Client);

            var types = await uom.GetTypes();

            ListTypes = types.ToList();

        }

        public List<UnitOfMeasureType> ListTypes = new List<UnitOfMeasureType>();
        public Task SelectModal(UnitOfMeasure DTO)
        {

            throw new NotImplementedException();
        }

        public IEnumerable<UnitOfMeasure> FilterList(string filter = "")
        {

            if (Grid != null && Grid.ItemsDataSource != null)
            {
                var templist = Grid.ItemsDataSource;

                //return null;

                return templist.Where(i => i.Name.ToLower().Contains(filter.ToLower()

                    )).ToArray();
            }
            else
            {
                return null;
            }
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

                    UOMServiceGRPC basics = new UOMServiceGRPC(Client);


                    Result = (await basics.Create(eq));


                    await ShowToast("Information Saved Successfully", ToastLevel.Success);

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




            var aa = (UnitOfMeasure)arg.Value;


            //CurrentEditContext = new EditContext(Grid.currentEdit);

            //bool re = await ContextValidation(true);


            //if (  re  || CustomValidation(eq))




            if (1 == 1)
            {


                try
                {

                    UOMServiceGRPC basics = new UOMServiceGRPC(Client);

                    aa.UnitOfMeasureBase = null;
                    aa.UncertaintyUnitOfMeasure = null;
                    aa.Type = null;

                    Result = (await basics.Create(aa));


                    await ShowToast("Information Saved Successfully", ToastLevel.Success);

                    await Grid.SearchFunction(Grid.currentColumn);

                    await CloseModal(Result);


                }

                catch (Exception ex)
                {


                    await ShowError("Error Save " + ex.Message);

                    throw ex;

                }

            }
        }

        protected async Task SubmittedUP(ChangeEventArgs arg)
        {

            var aa = (UnitOfMeasure)arg.Value;


            if (aa.UnitOfMeasureBase != null && (aa.UnitOfMeasureBaseID == null || aa.UnitOfMeasureBaseID == 0))
            {

                aa.UnitOfMeasureBaseID = aa.UnitOfMeasureBase.UnitOfMeasureID;
            }


            if (aa.UncertaintyUnitOfMeasure != null && (aa.UncertaintyUnitOfMeasureID == null || aa.UncertaintyUnitOfMeasureID == 0))
            {

                aa.UncertaintyUnitOfMeasureID = aa.UncertaintyUnitOfMeasure.UnitOfMeasureBaseID;
            }



            //CurrentEditContext = new EditContext(Grid.currentEdit);

            //bool re = await ContextValidation(true);


            //if (  re  || CustomValidation(eq))





            if (1 == 1)
            {

                try
                {

                    UOMServiceGRPC basics = new UOMServiceGRPC(Client);

                    aa.UnitOfMeasureBase = null;
                    aa.UncertaintyUnitOfMeasure = null;
                    aa.Type = null;

                    Result = (await basics.Update(aa));


                    await ShowToast("Information Saved Successfully", ToastLevel.Success);

                    await Grid.SearchFunction(Grid.currentColumn);

                    await CloseModal(Result);

                }
                catch (Exception ex)
                {
                    await ShowError("Error Save " + ex.Message);

                }

            }
        }
    }
}
