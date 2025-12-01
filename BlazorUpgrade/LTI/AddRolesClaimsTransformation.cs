using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Threading.Tasks;

namespace CalibrationSaaS.Infraestructure.Blazor
{
    public class AddRolesClaimsTransformation : IClaimsTransformation
    {
        //private readonly IUserService _userService;

        private Microsoft.Extensions.Configuration.IConfiguration config;
        public AddRolesClaimsTransformation(Microsoft.Extensions.Configuration.IConfiguration _config)
        {
            //_userService = userService;
            config = _config;

        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            // Clone current identity
            var clone = principal.Clone();
            var newIdentity = (ClaimsIdentity)clone.Identity;

            // Support AD and local accounts
            var nameId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier ||
                                                              c.Type == ClaimTypes.Name);
            if (nameId == null)
            {
                return principal;
            }

            // Get user from database
            //var user = await _userService.GetByUserName(nameId.Value);
            var url = config["Kestrel:Endpoints:Http3:Url"];

            //var user = await Program.GetUserById(nameId.Value,url);
            //if (user == null)
            //{
            //    return principal;
            //}

            // Add role claims to cloned identity
            //foreach (var role in user.RolesList2)
            //{
            //    //var claim = new Claim(role., role.Name);
            //    var  ad= newIdentity.Claims.Where(x=>x.Type==role.Type && x.Value == role.Value).FirstOrDefault();  
            //    if(ad == null)
            //    {
            //        newIdentity.AddClaim(role);
            //    }
                
            //}

            //foreach (var role in newIdentity.Claims)
            //{
            //    //var claim = new Claim(role., role.Name);
            //    var ad = user.Claims.Where(x => x.Type == role.Type && x.Value == role.Value).FirstOrDefault();
            //    if (ad == null)
            //    {
            //        newIdentity.RemoveClaim(role);
            //    }

            //}

            return clone;
        }
    }
}
