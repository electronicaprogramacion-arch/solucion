using Blazored.Modal;
using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Aggregates.Querys;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Grpc.Core;
using Helpers;
using Helpers.Controls;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blazed.Controls.Toast;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Security
{
    public class Security_CreateBase : Base_Create<User, Application.Services.IBasicsServices<CallContext>,
        Domain.Aggregates.Shared.AppStateCompany>
    {

        public EditContext RolEditContext { get; set; }

        public EditContext PermissionsEditContext { get; set; }

        public List<Rol> ListRol = new List<Rol>();
        public Rol Rol { get; set; } = new Rol();
        public ResponsiveTable<Rol> GridRol { get; set; } = new ResponsiveTable<Rol>();

        public ResponsiveTable<Rol> GridPermissions { get; set; } = new ResponsiveTable<Rol>();




        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                return;
            }

            NameValidationMessage = "valid";

            await LoadIni();


        }


        public async Task LoadIni()
        {
            if (EntityID != "0")
            {

                //User dto = new User();
                //dto.UserID = Convert.ToInt32(EntityID);
                //BasicsServiceGRPC basics = new BasicsServiceGRPC(Client);
                //var res = await basics.GetUserById(dto, new CallContext());
                //eq = res;
                //if (!string.IsNullOrEmpty(eq.PassWord))
                //{
                //    PassWordConfirm = eq.PassWord;
                //}

                //AppState.Users.Where(x => x.UserID == Convert.ToInt32(EntityID)).FirstOrDefault();
            }
            else
            {
                eq = new User();
                eq.UserRoles = new List<User_Rol>();
                eq.IsEnabled = true;


            }

            if (eq.UserRoles == null)
            {
                eq.UserRoles = new List<User_Rol>();
            }
            if (eq != null)
            {

                CurrentEditContext = new EditContext(eq);


                StateHasChanged();
            }
        }

        //public TechnicianCodeComponent Code = new TechnicianCodeComponent();

        [Inject] public Application.Services.IBasicsServices<CallContext> _basicsServices { get; set; }

        public string PassWordConfirm { get; set; }
        protected async Task FormSubmitted(EditContext editContext)
        {

            BasicsServiceGRPC basics = new BasicsServiceGRPC(Client);

            try

            {

                await ShowProgress();

                await SaveRol();

                var validate = await ContextValidation(true);


                if (validate)
                {

                    if (string.IsNullOrEmpty(eq.UserName) || string.IsNullOrEmpty(eq.PassWord) || string.IsNullOrEmpty(PassWordConfirm))
                    {
                        await ShowToast("Please review your credentials.", ToastLevel.Warning);

                    }


                    if (eq.PassWord != PassWordConfirm)
                    {

                        await ShowToast("Password and password confirm does not match ", ToastLevel.Warning);

                    }



                    if (GridPermissions?.Items?.Count > 0)
                    {
                        eq.RolesList = GridPermissions.Items;
                    }
                    //await ShowModal();


                    LastSubmitResult = "OnValidSubmit was executed";

                    // Result = (await Client.Create(eq));


                    var resVal = await basics.GetUserById2(eq);
                    if (resVal.Email != null && eq.UserID == 0)
                    {
                        await ShowToast("email already exists", ToastLevel.Error);
                        return;
                    }
                    Result = await basics.CreateUser(eq);


                }


                //if (GridRol.Items != null && GridRol.Items.Count > 0)
                //{

                //    foreach (var item in GridRol.Items)
                //    {
                //        await basics.AddRol(item);
                //    }

                //}



                //var a = await basics.GetRoles();
                //if (a != null && a.Roles != null)
                //{

                //    AppSecurity.RolesList = a.Roles;
                //    ListRol = a.Roles;
                //}

                await ShowToast("The information has been saved successfully.", level: ToastLevel.Success);



                await CloseModal();
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




            //ShowResult();


            //await CloseProgress();

        }


        public async Task SaveRol()
        {
            BasicsServiceGRPC basics = new BasicsServiceGRPC(Client);

            try
            {
                if (GridRol.Items != null && GridRol.Items.Count > 0)
                {

                    foreach (var item in GridRol.Items)
                    {
                        await basics.AddRol(item);
                    }

                }



                var a = await basics.GetRoles();
                if (a != null && a.Roles != null)
                {

                    AppSecurity.RolesList = a.Roles;
                    ListRol = a.Roles;
                }

                await ShowToast("The information has been saved successfully.", level: ToastLevel.Success);
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


            }
        }


        public List<KeyValue> PermissionsList = new List<KeyValue>();
        public User_Rol User_Rol { get; set; } = new User_Rol();

        public ModalParameters parameters = new ModalParameters();
        protected override async Task OnInitializedAsync()
        {
            

            RolEditContext = new EditContext(Rol);

            PermissionsEditContext = new EditContext(User_Rol);

            EntityID = 0.ToString();



            parameters.Add("SelectOnly", true);
            parameters.Add("IsModal", true);

            await base.OnInitializedAsync();

            TypeName = "User";

            //eq = null;


            //CurrentEditContext.OnFieldChanged += HandleFieldChanged;
            if (AppSecurity.RolesList == null)
            {
                BasicsServiceGRPC bas = new BasicsServiceGRPC(Client);

                var a = await bas.GetRoles();
                if (a != null && a.Roles != null)
                {

                    AppSecurity.RolesList = a.Roles;
                }
            }



            if (AppSecurity.RolesList != null)
            {
                ListRol = AppSecurity.RolesList.ToList();

            }

            if (AppSecurity?.Permissions?.Count == 0)
            {

                AppSecurity.Permissions = Enum.GetNames(typeof(Policies)).ConvertListStringinKeyValue();

            }

            PermissionsList = AppSecurity.Permissions.ToList();


            //var aa= PrerenderRouteHelper.GetRoutesToRender(typeof(CalibrationSaaS.Infraestructure.Blazor.App).Assembly) // Pass in the WebAssembly app's Assembly
            //.Select(config => new object[] { config });

            //var b = aa;

            //await LoadIni();


        }


        public async Task Delete(User DTO)
        {

            try
            {

                BasicsServiceGRPC basics = new BasicsServiceGRPC(Client);
                var user = await basics.DeleteUser(DTO, new CallContext());
                await CloseProgress();
                await ShowToast("Delete Item " + user.Name, ToastLevel.Success);


            }
            catch (Exception ex)
            {
                await ExceptionManager(ex);

            }


        }

        public async Task Change()
        {
            try
            {

                BasicsServiceGRPC basics = new BasicsServiceGRPC(Client);
                var user = await basics.Enable(eq, new CallContext());
                await CloseProgress();
                await ShowToast("The item has been enabled. " + eq.Name, ToastLevel.Success);


            }
            catch (Exception ex)
            {
                await ExceptionManager(ex);

            }
        }

        public async Task ChangeReset()
        {
            try
            {

                BasicsServiceGRPC basics = new BasicsServiceGRPC(Client);
                var user = await basics.Reset(eq, new CallContext());
                await CloseProgress();
                await ShowToast("The item has been reset " + eq.Name, ToastLevel.Success);


            }
            catch (Exception ex)
            {
                await ExceptionManager(ex);

            }
        }

        public new void Dispose()
        {

        }

    }
}
