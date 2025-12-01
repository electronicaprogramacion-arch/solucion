using Grpc.Core;
using ProtoBuf.Grpc;
using System.Linq;
using System.Security.Claims;
using Helpers.Controls;
using Helpers.Controls.ValueObjects;
namespace CalibrationSaaS.Infraestructure.GrpcServices.Services
{
    public class ServiceBase
    {

        public ClaimsPrincipal GetUser(CallContext context) 
        {
            var user = context.ServerCallContext.GetHttpContext().User;

            var aa= user.Claims.Where(x => x.Type == System.Security.Claims.ClaimTypes.Name).FirstOrDefault();

            if (aa == null) { 
             aa= user.Claims.Where(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier).FirstOrDefault();
            }

            return user;
                
                
         }

        public string GetUserName(ClaimsPrincipal user) 
        {
            

            var aa= user.Claims.Where(x => x.Type == System.Security.Claims.ClaimTypes.Name).FirstOrDefault();

            if (aa == null) { 
             aa= user.Claims.Where(x => x.Type == System.Security.Claims.ClaimTypes.NameIdentifier).FirstOrDefault();
            }

            return aa.Value;
                
                
         }

        public bool IsAuthenticate(CallContext context)
        {
            var a = GetUser(context);

            if (a!= null && a.Identity.IsAuthenticated)
            {
                return false;
            }
            else
            {
                return true;
            }

        }


    }
}
