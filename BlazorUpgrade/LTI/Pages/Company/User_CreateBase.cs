using Blazed.Controls;
using Blazed.Controls.Toast;
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
using Component = Helpers.Controls.Component;
namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Company
{
    public class User_CreateBase : Base_Create<User, Application.Services.IBasicsServices<CallContext>,
        Domain.Aggregates.Shared.AppStateCompany>
    {

        public EditContext RolEditContext { get; set; }

        public EditContext PermissionsEditContext { get; set; }
        public bool? pass;
        public List<Rol> ListRol = new List<Rol>();
        public Rol Rol { get; set; } = new Rol();
        public ResponsiveTable<Rol> GridRol { get; set; } = new ResponsiveTable<Rol>();

        public ResponsiveTable<Rol> GridPermissions { get; set; } = new ResponsiveTable<Rol>();




        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //if (pass.HasValue == true && pass.Value == true)
            //{
            //    if (!eq.PassWord.Contains(PassWordConfirm))
            //    {
            //        await ShowError("Password and password confirm does not match ");
            //    }
            //}
            if (!firstRender)
            {
                return;
            }

            

            NameValidationMessage = "valid";


            if (EntityID != "0")
            {

                User dto = new User();
                dto.UserID = Convert.ToInt32(EntityID);
                BasicsServiceGRPC basics = new BasicsServiceGRPC(Client);
                var res = await basics.GetUserById(dto, new CallContext());

                if (res == null)
                {
                    await ShowError("User not find");

                    return;
                }

                eq = res;
                if (!string.IsNullOrEmpty(eq.PassWord))
                {
                    PassWordConfirm = eq.PassWord;
                }

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
                Code.User = eq;

                StateHasChanged();
            }

        }




        public TechnicianCodeComponent Code = new TechnicianCodeComponent();

        [Inject] public Application.Services.IBasicsServices<CallContext> _basicsServices { get; set; }

        public string PassWordConfirm { get; set; }
        protected async Task FormSubmitted(EditContext editContext)
        {
            try

            {

                

                await ShowProgress();

                var validate = await ContextValidation(true);


                if (string.IsNullOrEmpty(eq.LastName))
                {
                    await ShowError("Last Name");
                    return;
                }


                //if (string.IsNullOrEmpty(eq.UserName) || string.IsNullOrEmpty(eq.PassWord) || string.IsNullOrEmpty(PassWordConfirm))
                //{
                //    await ShowError("Assign Roles, UserName, Permission & Password is required");
                //    return;
                //}

                //if (eq.PassWord != PassWordConfirm)
                //{

                //    await ShowError("Password and password confirm does not match ");
                //    return;
                //}


                if (Code.Grid.Items?.Count > 0)
                {

                    eq.TechnicianCodes = Code.Grid.Items;

                }

             

                if (validate)
                {
                    if (GridPermissions?.Items?.Count > 0)
                    {
                        eq.RolesList = GridPermissions.Items;
                    }
                    //await ShowModal();


                    LastSubmitResult = "OnValidSubmit was executed";

                    // Result = (await Client.Create(eq));
                    BasicsServiceGRPC basics = new BasicsServiceGRPC(Client);

                    var resVal = await basics.GetUserById2(eq);
                    if (resVal?.Email != null && eq?.UserID == 0)
                    {
                        await ShowToast("email already exists", ToastLevel.Error);
                        return;
                    }

                    Result = await basics.CreateUser(eq);



                    await ShowToast("The information has been saved successfully.", level: ToastLevel.Success);



                    await CloseModal();

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


            //ShowResult();


            //await CloseProgress();

        }

        public List<KeyValue> PermissionsList = new List<KeyValue>();
        public User_Rol User_Rol { get; set; } = new User_Rol();

        public Component component2 = new Component();

        protected override async Task OnInitializedAsync()
        {
             await base.OnInitializedAsync();

            RolEditContext = new EditContext(Rol);

            PermissionsEditContext = new EditContext(User_Rol);

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

            component2.Group = Component.Group;
            component2.Route = Component.Route + "_" + GridRol.GridID;
            component2.IsModal = true;
            component2.Permission = Component.Permission;


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



        public new void Dispose()
        {

        }

    }
}
