using CalibrationSaaS.Domain.Aggregates.Entities;
using ProtoBuf.Grpc;

namespace CalibrationSaaS.Infraestructure.Blazor.Pages.Basics
{
    public class ResolutionComponentBase : Base_Create<EquipmentTemplate, Application.Services.IBasicsServices<CallContext>, Domain.Aggregates.Shared.Basic.AppState>
    {



    }
}