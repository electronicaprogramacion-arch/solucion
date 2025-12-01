using System.Threading.Tasks;

namespace CalibrationSaaS.Reports.Infraestructure.APIRep.Services.Meta
{
    public interface ITemplateService
    {
        Task<string> RenderAsync<TViewModel>(string templateFileName, TViewModel viewModel);
    }
}