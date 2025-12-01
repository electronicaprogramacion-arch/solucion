using Blazed.Controls;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Infraestructure.Blazor.Services;
using Helpers.Controls;
using Helpers.Controls.ValueObjects;
using Microsoft.AspNetCore.Components;
using ProtoBuf.Grpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Component = Helpers.Controls.Component;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Company
{
    public partial class TechnicianCodeComponent:ComponentBase
    {

        [Inject] public Domain.Aggregates.Shared.AppSecurity AppSecurity { get; set; }
        [Inject] public Domain.Aggregates.Shared.AppStateCompany AppState { get; set; }

    [Inject] public Application.Services.IBasicsServices<CallContext> Service { get; set; }

    [Parameter]
    public User User { get; set; }

    [Parameter]
    public bool Enabled { get; set; }

    protected async Task Submitted(ChangeEventArgs arg)
    {


    }

    protected async Task SubmittedUP(ChangeEventArgs arg)
    {


    }

    public async Task<bool> Delete(TechnicianCode DTO)
    {
        BasicsServiceGRPC basic = new BasicsServiceGRPC(Service);

        var result = await basic.DeleteTechnicianCode(DTO);

        return true;
    }


    public async Task<ResultSet<TechnicianCode>> LoadData(Pagination<TechnicianCode> pag)
    {

        BasicsServiceGRPC basic2 = new BasicsServiceGRPC(Service);

        pag.Entity = new TechnicianCode();

        pag.Entity.User = User;
        //YPPP 03/02/2021
        pag.Entity.UserID = User.UserID;
        var result = await basic2.GetTechnicianCodePag(pag);

        return result;

    }


    public ICollection<Certification> Certifications { get; set; }



    public string ConvertState(StateLocation state)
    {

        var p = state.Value;
        return p;
    }
    public StateLocation LookupState(string id, TechnicianCode model)
    {
        var res = AppState.States.FirstOrDefault(p => p.Value == id);
        if (model != null && res != null)
        {
            //model.StateID = res.Name;
        }
        return res;
    }




    public Task<IEnumerable<StateLocation>> SearchState(string searchText)
    {
        var result = AppState.States
            .Where(x => x.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0 || (x.Value != null && x.Value.ToLower().Contains(searchText)))
            .ToList();

        return Task.FromResult<IEnumerable<StateLocation>>(result);
    }


    public bool LabelDown { get; set; }

    public TechnicianCode TechnicianCode { get; set; }

    List<TechnicianCode> _Group;


    public List<TechnicianCode> Group
    {

        get
        {
            return _Group;
        }
        set
        {

            _Group = value;


        }
    }

    public ResponsiveTable<TechnicianCode>
          Grid
    { get; set; } = new ResponsiveTable<TechnicianCode>();

        [Parameter]
    public Component Component { get; set; } = new Component();

    protected override async Task OnInitializedAsync()
    {
        //return base.OnInitializedAsync();

       

               

            await base.OnInitializedAsync();



            BasicsServiceGRPC basic = new BasicsServiceGRPC(Service);

            var result = await basic.GetCertifications();

            Certifications = result as ICollection<Certification>;

        }



    }
}
