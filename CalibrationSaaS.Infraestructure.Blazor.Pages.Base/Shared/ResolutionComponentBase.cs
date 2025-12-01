using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Infraestructure.Blazor.Pages.Basics;
using ProtoBuf.Grpc;

namespace BlazorApp1.Blazor.Blazor.Pages.Basics
{
    public class ResolutionComponentBase : Base_Create<EquipmentTemplate, CalibrationSaaS.Application.Services.IBasicsServices<CallContext>, CalibrationSaaS.Domain.Aggregates.Shared.Basic.AppState>
    {



    }
}