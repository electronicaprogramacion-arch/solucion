using Blazed.Controls;
using Blazed.Controls.Toast;
using Blazor.Extensions.Logging;

using Blazored.Modal;
using Blazored.Modal.Services;
using BlazorInputFile;
using CalibrationSaaS.Application.Services;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Shared.Basic;
using BlazorApp1.Blazor.Blazor.Pages.AssetsBasics;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Grpc.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BlazorApp1.Blazor.Pages.Customer
{
    public class Customer_CreateBase :
        Base_Create<CalibrationSaaS.Domain.Aggregates.Entities.Customer, Func<dynamic, ICustomerService<CallContext>>, CalibrationSaaS.Domain.Aggregates.Shared.AppStateCompany>
    {

#pragma warning disable CS0108 // 'Customer_CreateBase.Logger' oculta el miembro heredado 'KavokuComponentBase<Customer>.Logger'. Use la palabra clave new si su intención era ocultarlo.
        [Inject] ILogger<Customer_CreateBase> Logger { get; set; }
#pragma warning restore CS0108 // 'Customer_CreateBase.Logger' oculta el miembro heredado 'KavokuComponentBase<Customer>.Logger'. Use la palabra clave new si su intención era ocultarlo.
#pragma warning disable CS0108 // 'Customer_CreateBase.JSRuntime' oculta el miembro heredado 'KavokuComponentBase<Customer>.JSRuntime'. Use la palabra clave new si su intención era ocultarlo.
        [Inject] IJSRuntime JSRuntime { get; set; }
#pragma warning restore CS0108 // 'Customer_CreateBase.JSRuntime' oculta el miembro heredado 'KavokuComponentBase<Customer>.JSRuntime'. Use la palabra clave new si su intención era ocultarlo.

        public CustomerGRPC _customerGrpc { get; set; }

        public ResponsiveTable<CalibrationSaaS.Domain.Aggregates.Entities.User>
           ContactsRazor
        { get; set; }


        public ResponsiveTable<CalibrationSaaS.Domain.Aggregates.Entities.Social>
         SocialComponent
        { get; set; }


        public ResponsiveTable<CalibrationSaaS.Domain.Aggregates.Entities.PhoneNumber>
         PhoneNumbersComponent
        { get; set; }

        public BlazorApp1.Blazor.Pages.Customer.Address_Create
         AddresComponent
        { get; set; }

        public BlazorApp1.Blazor.Pages.Customer.Contact_Create
         ContactComponent
        { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int _CustomerId { get; set; }
        public string _CustomerValue { get; set; }

        public int? _EquipmentTemplateID { get; set; }
     
        public string _Name { get; set; }

        public CalibrationSaaS.Domain.Aggregates.ValueObjects.AddressResultSet _addressFiltered;

        public bool isDisable { get; set; }

        [CascadingParameter]
        public IModalService Modal { get; set; } = default!;

        [Parameter]
        public string CustomerId { get; set; }

        [Parameter]
        public string CustomerName { get; set; }
        [Parameter]
        public int AddressId { get; set; }

        [Parameter]
        public string AddressStreet { get; set; }

        public bool SelectedUser { get; set; } = true;

        [Inject] public Func<dynamic, CalibrationSaaS.Application.Services.IAssetsServices<CallContext>> _assetsServices { get; set; }

        public CalibrationSaaS.Domain.Aggregates.Entities.Customer eqNew = new CalibrationSaaS.Domain.Aggregates.Entities.Customer();
        /// <summary>

        public BlazorApp1.Blazor.Blazor.Pages.AssetsBasics.POEByEqTemplate_Search PieceOfEquipmentSearch = new POEByEqTemplate_Search();

        public ICollection<CalibrationSaaS.Domain.Aggregates.Entities.PieceOfEquipment> _listPieceOfEquipment = new List<CalibrationSaaS.Domain.Aggregates.Entities.PieceOfEquipment>();

        public List<CalibrationSaaS.Domain.Aggregates.Entities.Customer> _customerList = new List<CalibrationSaaS.Domain.Aggregates.Entities.Customer>();
        
        [Parameter]
        public CalibrationSaaS.Domain.Aggregates.Entities.Customer Customer { get; set; }
        
        public CustomerAggregate _cust = new CustomerAggregate();
        List<CustomerAggregate> _listAggregate = new List<CustomerAggregate>();
        [Inject] public CalibrationSaaS.Application.Services.IPieceOfEquipmentService<CallContext> _poeServices { get; set; }
      
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {


                if (firstRender)
                {
                    await ShowProgress();

                    NameValidationMessage = "valid";

                    if (EntityID == "0")
                    {

                        eq = new CalibrationSaaS.Domain.Aggregates.Entities.Customer();


                    }
                    else
                    {
                        eq.CustomerID = Convert.ToInt32(EntityID);
                        _customerGrpc = new CustomerGRPC(Client,DbFactory);
                        eq = await _customerGrpc.GetCustomersByID(eq, new CallOptions());

                        PieceOfEquipmentGRPC POEbasics = new PieceOfEquipmentGRPC(_poeServices);
                        var res = await POEbasics.GetPieceOfEquipmentByCustomerId(eq);



                        if (res?.PieceOfEquipments != null)
                        {
                            Console.WriteLine(res.PieceOfEquipments.Count);
                            _listPieceOfEquipment = res.PieceOfEquipments; //eq.PieceOfEquipments;
                            PieceOfEquipmentSearch.LIST1 = _listPieceOfEquipment.ToList();
                            PieceOfEquipmentSearch.Show(_listPieceOfEquipment.ToList());
                        }



                    }
                    
                    if ((eq != null && eq.Aggregates == null) || eq.Aggregates.Count == 0)
                    {
                        List<CustomerAggregate> agglist = new List<CustomerAggregate>();

                        CustomerAggregate agg = new CustomerAggregate();

                        agglist.Add(agg);

                        eq.Aggregates = agglist;
                    }

                    if (eq != null)
                    {
                        Console.WriteLine(eq.Name);
                        CurrentEditContext = new EditContext(eq);
                        StateHasChanged();
                    }

                    await RemoveValidateClass();
                }

                await base.OnAfterRenderAsync(firstRender);
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
                await CloseProgress();
            }




        }

        public async Task ShowModalCustomer()
        {
            Dictionary<string, object> EmptyValidationDictionary = new Dictionary<string, object>();

            var parameters = new ModalParameters();
            parameters.Add("SelectOnly", true);
            parameters.Add("IsModal", true);
            ModalOptions op = new ModalOptions();
            ////op.ContentScrollable = true;
            op.Class = "blazored-modal " + Blazed.Controls.ModalSize.MediumWindow;

            var messageForm = Modal.Show<Customer_Search>("Select Customer", parameters, op);
            var result = await messageForm.Result;

            if (!result.Cancelled)
            {

                eqNew = (CalibrationSaaS.Domain.Aggregates.Entities.Customer)result.Data;

                EmptyValidationDictionary = result.Data.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                      .ToDictionary(prop => prop.Name, prop => prop.GetValue(result.Data, null));

                _CustomerId = eqNew.CustomerID;
                _CustomerValue = eqNew.Name;
                //Customer = eqNew;
                
                CalibrationSaaS.Domain.Aggregates.Entities.Customer customer = new CalibrationSaaS.Domain.Aggregates.Entities.Customer()
                {
                    CustomerID = _CustomerId,
                    Name = _CustomerValue
                };
                //eq.CustomerId = eq.CustomerID;
                _customerList.Add(customer);

                await CustomerChange(_CustomerId.ToString());
                
            }
        }

        
        public async Task<bool> Delete(CalibrationSaaS.Domain.Aggregates.Entities.Social DTO)
        {
            try
            {

             
                CustomerGRPC basic = new CustomerGRPC(Client,DbFactory);

                var result = await basic.DeleteSocial(DTO);

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

        public async Task<bool> DeletePhone(CalibrationSaaS.Domain.Aggregates.Entities.PhoneNumber DTO)
        {
            try
            {
                if (DTO.PhoneNumberID == 0 && DTO.Number != null )
                {
                    _listAggregate.Clear();
                    var listPhone = PhoneNumbersComponent.Items.Where(x=>x.Number != DTO.Number).ToList();
                    _cust.PhoneNumbers = listPhone;


                    _listAggregate.Add(_cust);
                    eq.Aggregates = _listAggregate;
                }
                else
                { 
             
                CustomerGRPC basic = new CustomerGRPC(Client,DbFactory);

                var result = await basic.DeletePhone(DTO);
                _listAggregate.Clear();
                if (PhoneNumbersComponent.Items.Count > 0)
                {
                    
                    var listPhone = PhoneNumbersComponent.Items;
                    _cust.PhoneNumbers = listPhone;

                    
                    _listAggregate.Add(_cust);
                    eq.Aggregates = _listAggregate;

                }

                }
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

        public EditContext phoneEditContext { get; set; }

        public EditContext SocialEditContext { get; set; }

        protected async Task FormSubmitted(EditContext editContext)
        {
            try
            {
                Logger.LogDebug("herherhere");


                await ShowProgress();

                //customValidator.ClearErrors();

                //var errors = new Dictionary<string, List<string>>();

                var validate = await ContextValidation(true);


                if (validate || CustomValidation(eq))
                {



                    if ((ContactComponent.Grid.Items != null && ContactComponent.Grid.Items.Count > 0)
                    || (AddresComponent.RT.Items != null && AddresComponent.RT.Items.Count > 0)
                    || (PhoneNumbersComponent.Items != null && PhoneNumbersComponent.Items.Count > 0)
                    || (SocialComponent.Items != null && SocialComponent.Items.Count > 0)
                    )
                    {
                        
                        

                        _cust.Name = "Test";

                        if (eq.Aggregates == null)
                        {
                            // _listAggregate.Add(_cust);
                            eq.Aggregates = _listAggregate;


                        }
                        else
                        {
                            _cust = eq.Aggregates.ElementAtOrDefault(0);
                        }


                        //eq.Aggregates.ToList().ForEach(item =>
                        //{
                        //    item.Addresses=
                        //});


                        if (ContactComponent?.Grid?.Items?.Count > 0)
                        {
                            var listContacts = ContactComponent.Grid.Items;
                            _cust.Contacts = listContacts;
                            //_listAggregate.Add(_cust);
                            //eq.Aggregates = _listAggregate;
                        }

                        if (AddresComponent?.RT?.Items?.Count > 0)
                        {
                            var listAddress = AddresComponent.RT.Items;
                            _cust.Addresses = listAddress;


                        }

                        if (PhoneNumbersComponent?.Items?.Count > 0)
                        {
                            var listPhone = PhoneNumbersComponent.Items;
                            _cust.PhoneNumbers = listPhone;


                        }

                        if (SocialComponent?.Items?.Count > 0)
                        {
                            var listSocial = SocialComponent.Items;
                            _cust.Socials = listSocial;


                        }

                        _cust.Name = "Test";

                        _listAggregate.Add(_cust);
                        eq.Aggregates = _listAggregate;

                    }
                    else if (ContactComponent.Grid.Items == null || ContactComponent.Grid.Items.Count == 0)
                    {

                        await ShowToast("No Contacts Configurated", ToastLevel.Warning);


                        //return ;
                    }
                    else if (AddresComponent.RT.Items == null || AddresComponent.RT.Items.Count == 0)
                    {
                        await ShowToast("No Address Configurated", ToastLevel.Warning);

                        //return;
                    }
                    else if (PhoneNumbersComponent.Items == null || PhoneNumbersComponent.Items.Count == 0)
                    {
                        await ShowToast("No Phone Configurated", ToastLevel.Warning);
                        //eq.Aggregates = null;
                        //return;
                    }




                    LastSubmitResult = "OnValidSubmit was executed";
                    _customerGrpc = new CustomerGRPC(Client,DbFactory);


                    Result = (await _customerGrpc.CreateCustomer(eq, new CallOptions()));
                    await ShowToast("The information has been saved successfully.", ToastLevel.Success);

                   
                    eq.CustomerID = Result.CustomerID;
                    await CloseModal(Result);

                    //StateHasChanged();
                    if ((eq != null && Result.Aggregates != null) || eq.Aggregates.Count > 0)
                    {
                        List<CustomerAggregate> agglist = new List<CustomerAggregate>();
                                              

                        agglist = Result.Aggregates.ToList();
                        

                        eq.Aggregates = agglist;
                    }

                    if (eq != null)
                    {
                        Console.WriteLine(eq.Name);
                        CurrentEditContext = new EditContext(eq);
                        StateHasChanged();
                    }

                    await RemoveValidateClass();



                }

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
                await CloseProgress();
            }





        }

#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        protected async Task InvalidFormSubmitted(EditContext editContext)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica. Puede usar el operador 'await' para esperar llamadas API que no sean de bloqueo o 'await Task.Run(...)' para hacer tareas enlazadas a la CPU en un subproceso en segundo plano.
        {
            LastSubmitResult = "OnInvalidSubmit was executed";
        }


        PhoneNumber phone = new PhoneNumber();

        public Social Social = new Social();

        protected override async Task OnInitializedAsync()
        {
            phoneEditContext = new EditContext(phone);

            SocialEditContext = new EditContext(Social);

            if (IsModal == true)
            {
                _CustomerId = Convert.ToInt32(CustomerId);
                _CustomerValue = CustomerName;
                isDisable = true;
                Console.Write("Customer Id = " + _CustomerId);

                AddressId = 1;
                await CustomerChange(_CustomerId.ToString());
                _addressFiltered.Addresses.FirstOrDefault().AddressId = AddressId;
                _addressFiltered.Addresses.FirstOrDefault().StreetAddress1 = AddressStreet;
                //eq.AddressId = AddressId;
                SelectedUser = false;

                //eq.Customer = Customer;

            }

            if (_addressFiltered?.Addresses != null)
            {
                var a = _addressFiltered.Addresses.Where(x => x.AddressId == AddressId).FirstOrDefault();
                if (a != null)
                {
                    AddressStreet = a.StreetAddress1 + "| " + a.CityID + " " + a.StateID;

                    SelectedUser = false;

                    //eq.AddressId = AddressId;


                }
            }

            await base.OnInitializedAsync();          

            TypeName = "Customer";

            NameValidationMessage = "valid";

        }
        public async Task CustomerChange2(CalibrationSaaS.Domain.Aggregates.Entities.Customer _custId)
        {
            if (Client != null)
            {
                AssetsServiceGRPC assetsServiceGRPC = new AssetsServiceGRPC(_assetsServices, DbFactory);

                _addressFiltered = await assetsServiceGRPC.GetAddressByCustomerId(_custId);


            }
        }


        public async Task CustomerChange(string customerId)
        {
            if (Client != null)
            {
                AssetsServiceGRPC ass = new AssetsServiceGRPC(_assetsServices, DbFactory);

                CalibrationSaaS.Domain.Aggregates.Entities.Customer _custId = new CalibrationSaaS.Domain.Aggregates.Entities.Customer();
                _custId.CustomerID = Convert.ToInt32(customerId);
                _addressFiltered = await ass.GetAddressByCustomerId(_custId);
            }
        }

        protected void FunctionName(EventArgs args)
        {
            //PropertyInfo myPropInfo = eq.GetType().GetProperty("MyProperty");

            if (eq.Name.Length < 1)
            {
                NameValidationMessage = "invalid";
            }
            else
            {
                NameValidationMessage = "valid";
            }
            Logger.LogDebug(this.eq.Name);
            StateHasChanged();

        }



        public new void Dispose()
        {

        }

 public PhoneNumber newPhone()
    {

        PhoneNumber p = new PhoneNumber();

        p.IsEnabled = true;

        return p;

    }

    public PhoneNumber PhoneNumber = new PhoneNumber();

    //ResponsiveTable<PhoneNumber> GridPhone = new ResponsiveTable<PhoneNumber>();

    //Social Social = new Social();

    //private void HandleValidSubmit()
    //{
    //    child.ShowModal("hello", "valid", false, false, "close");
    //}

    private void HandleReset()
    {
        eq = new CalibrationSaaS.Domain.Aggregates.Entities.Customer();
        CurrentEditContext = new EditContext(eq);
    }

    public ResponsiveTable<User> Grid { get; set; }

    string selectedAnswer = "";

    bool IsPercentage = false;

    void SelectionChanged(ChangeEventArgs args)
    {




        selectedAnswer = args.Value.ToString();

        Logger.LogDebug(selectedAnswer);

        if (selectedAnswer.Contains("Percentage"))
        {
            IsPercentage = true;
        }
        else
        {
            IsPercentage = false;
        }

        StateHasChanged();
    }


    void OnSelect(ChangeEventArgs e)
    {
        Logger.LogDebug("OnSelect");
        e.Value.ToString();
    }


    public void Mchange(object o, ChangeEventArgs e)
    {
        Logger.LogDebug("Mchange");
    }

        public async Task ReplaceCustomer()
        {
            CustomerReplaced customerReplaced = new CustomerReplaced();
            customerReplaced.ListPieceOfEquipment = _listPieceOfEquipment.ToList();
            customerReplaced.CustomerOld = eq;
            var customerNew = new CalibrationSaaS.Domain.Aggregates.Entities.Customer();
           
            customerNew.CustomerID = eqNew.CustomerID;
            customerReplaced.CustomerNew = customerNew;

            _customerGrpc = new CustomerGRPC(Client, DbFactory);


            var result = await _customerGrpc.ReplaceCustomer(customerReplaced, new CallOptions());

            eq = result;

            StateHasChanged();
        }

        public string Country { get; set; }

        protected bool IsThermotempCustomer()
        {
            try
            {
                string customer = Configuration.GetSection("Reports")?["Customer"];
                return customer == "Thermotemp";
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error checking Thermotemp customer configuration");
                return false;
            }
        }

    }
}
