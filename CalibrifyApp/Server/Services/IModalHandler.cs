using Blazored.Modal.Services;
using System.Threading.Tasks;

namespace CalibrifyApp.Server.Services
{
    public interface IModalHandler
    {
        Task<string> ShowModalAsync(string jsonTolerancePerTestPoint);
    }
}
