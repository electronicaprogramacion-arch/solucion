using CalibrationSaaS.Domain.Aggregates.Entities;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Helpers.Controls;
using Policies = Helpers.Controls.Policies;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.NetworkInformation;
using IdentityModel;
using Microsoft.AspNetCore.Http;





namespace CalibrationSaaS.Domain.Aggregates.Security
{
    public class AccesHandler : AuthorizationHandler<AccesRequirement>
    {
        public bool IsInRole2(ClaimsPrincipal user, string roles,string acces)
        {

            var a = user.Claims.Where(x =>x.Type== roles && x.Value == acces).FirstOrDefault();

            return true;
        
        }

            public bool IsInRole(ClaimsPrincipal user, string roles, out bool specified)
        {
            var s = false;

            specified = false;
            var ra = roles.Split(",", StringSplitOptions.RemoveEmptyEntries);

            foreach(var ite in ra)
            {
                if (ite.Contains("."))
                {
                    specified = true;
                    var ril = ite.Split(".")[0];
                    s = user.IsInRole(ril);
                    if (!s)
                    {
                        s = IsInRole2(user, JwtClaimTypes.Role, ril);
                    }
                    if (s)
                    {
                        break;
                    }
                }
                else
                {
                    s = user.IsInRole(ite.Trim());
                    if (!s)
                    {
                        s = IsInRole2(user, JwtClaimTypes.Role, ite.Trim());
                    }
                    if (s)
                    {
                        break;
                    }
                }
               
            }

            return s;
        }

        public bool FindRol(string roles, string word)
        {

            var ra = roles.Split(",", StringSplitOptions.RemoveEmptyEntries);

            foreach (var ite in ra)
            {
                var ra1= ite.Split(".", StringSplitOptions.RemoveEmptyEntries);

                foreach (var ite2 in ra1)
                {
                    if (word == ite2)
                    {
                        return true;
                    }
                }
               


            }

            return false;
        }


        public bool IsInRole(ClaimsPrincipal user, string roles,string acces)
        {
            var s = false;


            var ra = roles.Split(",", StringSplitOptions.RemoveEmptyEntries);

            foreach (var ite in ra)
            {                
                if (ite.Contains("."))
                {
                    s = user.IsInRole(ite.Trim());
                    if (!s)
                    {
                        s = IsInRole2(user, JwtClaimTypes.Role, ite.Trim());
                    }                  

                    if (s)
                    {
                        break;
                    }
                }
                else
                {
                    var perm = ite.Trim() + "." + acces;
                    s = user.IsInRole(perm);
                    if (!s)
                    {
                        s = IsInRole2(user, JwtClaimTypes.Role, perm);
                    }
                    if (s)
                    {
                        break;
                    }
                }

                
            }

            return s;
        }






        protected override  Task HandleRequirementAsync(AuthorizationHandlerContext context, AccesRequirement requirement)
        {

            try
            {
                //AuthorizationHandlerContext
                //context.Succeed(requirement);

                //return Task.CompletedTask;


                if (string.IsNullOrEmpty(requirement.Requirement) || context.User.Identity== null || !context.User.Identity.IsAuthenticated)
                //    if (requirement.Requirement==null)
                {
                        context.Fail();
                        return Task.CompletedTask;
                }

                if (context == null)
                {
                    return Task.CompletedTask;
                }

                var user = context.User;


                if (user.IsInRole("disable"))
                {

                    AuthorizationFailureReason f = new AuthorizationFailureReason(this, "disable");

                    
                    context.Fail(f);                   

                    return Task.CompletedTask;


                }

                if (user.IsInRole("reset"))
                {

                    AuthorizationFailureReason f = new AuthorizationFailureReason(this, "reset");


                    context.Fail(f);

                    return Task.CompletedTask;


                }


                if (user.IsInRole("superadmin"))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }

                bool specified = false;
                string roles22 = requirement.Requirement; // "";
                //if(requirement.Requirement != null)
                //{
                //    foreach (var ity in requirement.Requirement)
                //    {
                //        roles22 = roles + ity.Name;
                //    }
                //}
                var rsou = context.Resource;

                if (IsInRole(user, roles22, out specified) && context.Resource == null 
                    || IsInRole(user, roles22, out specified) && context.Resource.GetType() == typeof(DefaultHttpContext))
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }

                //if (user.IsInRole("tech") && context.Resource == null)
                //{
                //    context.Succeed(requirement);
                //    return Task.CompletedTask;
                //}


                //&& context.Resource.GetType()==typeof(CalibrationSaaS.Domain.Aggregates.Entities.Component
                string componentname = "";
                bool isadminview = user.IsInRole("LTIadmin");
                if (context.Resource != null && context.Resource.GetType() == typeof(Helpers.Controls.Component))
                {

                   
                    var a = (Helpers.Controls.Component)context.Resource;

                    if(a == null || string.IsNullOrEmpty(a.Name))
                    {
                        AuthorizationFailureReason f = new AuthorizationFailureReason(this, "Component is null or empty");
                        context.Fail(f);
                        return Task.CompletedTask;
                    }

                    componentname = a.Name;

                    if (IsInRole(user,"admin",out specified) && a.Group.Trim() == "equipmenttype" && a.Permission == Helpers.Controls.Policies.HasView)
                    {
                        context.Succeed(requirement);
                        return Task.CompletedTask;
                    }
                    //if role is admin has permissions
                    if (user.IsInRole("admin") && a.Group.Trim() != "equipmenttype")
                    {
                        context.Succeed(requirement);
                        return Task.CompletedTask;
                    }

                    if(!string.IsNullOrEmpty(user?.Identity?.Name) && a.Permission == Policies.ViewAssignationsOnly && user?.Identity?.Name== a.User)
                    {
                        context.Succeed(requirement);
                        return Task.CompletedTask;
                    }
                    else if (!string.IsNullOrEmpty(user?.Identity?.Name) && a.Permission == Policies.ViewAssignationsOnly && user?.Identity?.Name != a.User)
                    {
                        context.Fail();
                        return Task.CompletedTask;
                    }

                    var isaut = IsInRole(user, a.Group.Trim(),out specified); // (a.Group.T8rim());

                    bool istech = FindRol(a.Group.Trim(), "tech");

                    bool isjob = FindRol(a.Group.Trim(), "job");

                    bool isjobview = user.IsInRole("LTIjob");

                    bool istechview = user.IsInRole("LTItech");

                    if (istechview)
                    {
                        istech = true;
                    }

                    string pro = "";

                    if (istechview)
                    {
                        pro = "LTItech";
                    }
                    if (isjobview)
                    {
                        pro = "LTIjob";
                    }
                    if (isadminview)
                    {
                        pro = "LTIadmin";
                    }
                    if (istech)
                    {
                        pro = "tech";
                    }



                    List<string> lstComponent = new List<string>();
                    lstComponent.Add("PieceOfEquipmentSearch");
                    lstComponent.Add("PieceOfEquipmentCreate");
                    lstComponent.Add("EquipmentTemplate");
                    lstComponent.Add("EquipmentTemplateSearch");

                    var claimsIdentity = (ClaimsIdentity)user.Identity;
                    var claims = claimsIdentity.Claims;


                    if (isjob)
                    {
                       

                        var cc33 = claims.Where(x => x.Type == JwtClaimTypes.Role && x.Value == "job.ReadyforCalibration").FirstOrDefault();
                        if (cc33 == null)
                        {
                            var claim55 = new Claim(JwtClaimTypes.Role, "job.ReadyforCalibration");
                            claimsIdentity.AddClaim(claim55);
                        }

                    }


                    if(isjob && a.Route.Contains("WODOff"))
                    {
                        context.Succeed(requirement);
                        return Task.CompletedTask;
                    }

                    if (istech && a.Route.Contains("WODOff"))
                    {
                        context.Succeed(requirement);
                        return Task.CompletedTask;
                    }

                    //if ((istech && !istechview) || (istechview && !(lstComponent.Any(x => x.Contains(componentname)))))

                        if ((istech ) )
                    {
                       

                        var cc = claims.Where(x => x.Type == JwtClaimTypes.Role && x.Value == pro + ".HasView").FirstOrDefault();

                        if (cc == null)
                        {
                            var claim2 = new Claim(JwtClaimTypes.Role, pro + ".HasView");
                            claimsIdentity.AddClaim(claim2);

                            
                        }

                        var cc1 = claims.Where(x => x.Type == JwtClaimTypes.Role && x.Value == pro + ".HasEdit").FirstOrDefault();

                        if (cc1 == null)
                        {
                            var claim3 = new Claim(JwtClaimTypes.Role, pro + ".HasEdit");
                            claimsIdentity.AddClaim(claim3);
                        }

                        var cc2 = claims.Where(x => x.Type == JwtClaimTypes.Role && x.Value == pro + ".HasSave").FirstOrDefault();
                        if ( cc2== null)
                        {
                            var claim4 = new Claim(JwtClaimTypes.Role, pro + ".HasSave");
                            claimsIdentity.AddClaim(claim4);
                        }

                        var cc3 = claims.Where(x => x.Type == JwtClaimTypes.Role && x.Value == pro + ".ReadyforCalibration").FirstOrDefault();
                        if ( cc3== null)
                        {
                            var claim5 = new Claim(JwtClaimTypes.Role, pro + ".ReadyforCalibration");
                            claimsIdentity.AddClaim(claim5);
                        }

                        var cc4 = claims.Where(x => x.Type == JwtClaimTypes.Role && x.Value == pro + ".ContractReview").FirstOrDefault();
                        if (cc4 == null)
                        {
                            var claim6 = new Claim(JwtClaimTypes.Role, pro + ".ContractReview");
                            claimsIdentity.AddClaim(claim6);
                        }

                        var cc5 = claims.Where(x => x.Type == JwtClaimTypes.Role && x.Value == pro + ".HasNew").FirstOrDefault();
                        if (cc5 == null)
                        {
                            var claim7 = new Claim(JwtClaimTypes.Role, pro + ".HasNew");
                            claimsIdentity.AddClaim(claim7);
                        }


                    }

                  


                    //lstComponent.Add();


                    //if (isadminview)
                    //{
                    //    var cc2 = claims.Where(x => x.Type == JwtClaimTypes.Role && x.Value == "LTIadmin.HasFullacces").FirstOrDefault();
                    //    if (cc2 == null)
                    //    {
                    //        var claim4 = new Claim(JwtClaimTypes.Role, "LTIadmin.HasFullacces");
                    //        claimsIdentity.AddClaim(claim4);
                    //    }
                    //}



                    //if ((istechview || isjobview || isadminview) && (lstComponent.Any(x => x.Contains(componentname))))
                    //{
                                        


                    //    var cc = claims.Where(x => x.Type == JwtClaimTypes.Role && x.Value == pro +".HasView").FirstOrDefault();

                    //    if (cc == null)
                    //    {
                    //        var claim2 = new Claim(JwtClaimTypes.Role, pro + ".HasView");
                    //        claimsIdentity.AddClaim(claim2);
                    //    }

                    //    //var cc1 = claims.Where(x => x.Type == JwtClaimTypes.Role && x.Value == "tech.HasEdit").FirstOrDefault();

                    //    //if (cc1 == null)
                    //    //{
                    //    //    var claim3 = new Claim(JwtClaimTypes.Role, "tech.HasEdit");
                    //    //    claimsIdentity.AddClaim(claim3);
                    //    //}

                    //    //var cc2 = claims.Where(x => x.Type == JwtClaimTypes.Role && x.Value == "tech.HasSave").FirstOrDefault();
                    //    //if (cc2 == null)
                    //    //{
                    //    //    var claim4 = new Claim(JwtClaimTypes.Role, "tech.HasSave");
                    //    //    claimsIdentity.AddClaim(claim4);
                    //    //}

                    //    var cc3 = claims.Where(x => x.Type == JwtClaimTypes.Role && x.Value == "techview.ReadyforCalibration").FirstOrDefault();
                    //    if (cc3 == null)
                    //    {
                    //        var claim5 = new Claim(JwtClaimTypes.Role, "techview.ReadyforCalibration");
                    //        claimsIdentity.AddClaim(claim5);
                    //    }

                    //    var cc4 = claims.Where(x => x.Type == JwtClaimTypes.Role && x.Value == "techview.ContractReview").FirstOrDefault();
                    //    if (cc4 == null)
                    //    {
                    //        var claim6 = new Claim(JwtClaimTypes.Role, "techview.ContractReview");
                    //        claimsIdentity.AddClaim(claim6);
                    //    }

                    //    //var cc5 = claims.Where(x => x.Type == JwtClaimTypes.Role && x.Value == "tech.HasNew").FirstOrDefault();
                    //    //if (cc5 == null)
                    //    //{
                    //    //    var claim7 = new Claim(JwtClaimTypes.Role, "tech.HasNew");
                    //    //    claimsIdentity.AddClaim(claim7);
                    //    //}


                    //}


                    if (isaut)
                    {
                        var ra = a.Group.Trim().Split(",", StringSplitOptions.RemoveEmptyEntries);

                        var vl = a.Permission.ToString();//Enum.Parse(typeof(Policies),);

                        foreach (var rol in ra)
                        {
                            foreach (var item in Enum.GetNames(typeof(Policies)))
                            {
                                //var vl = a.Permission.ToString();//Enum.Parse(typeof(Policies),);

                                if (vl.ToString().ToLower() == item.ToLower() )
                                {
                                    bool val = false;
                                    // var val = IsInRole(user, a.Group.Trim() + "." + item);//context.User.FindFirst(c => c.Type == a.Group.Trim() && c.Value == Policies.HasFullacces);
                                    if(rol.ToLower().Contains(".") && rol.Split(".")[1]==(item))
                                    {
                                        val = IsInRole(user, rol, item);
                                       

                                    }
                                    else if(!rol.ToLower().Contains("."))
                                    {
                                         val = IsInRole(user, rol, item);
                                        
                                    }
                                   

                                    if (val)
                                    {
                                        context.Succeed(requirement);
                                        return Task.CompletedTask;
                                    }
                                }

                            }
                           
                        }

                        //if (istech && !specified)
                        //{
                        //    context.Succeed(requirement);
                        //    return Task.CompletedTask;
                        //}

                        return Task.CompletedTask;
                    }
                   
                    else
                    {
                        AuthorizationFailureReason f = new AuthorizationFailureReason(this, "You not have access to see this resource");

                        context.Fail(f);
                        return Task.CompletedTask;
                    }


                }

                else
                {
                    AuthorizationFailureReason f = new AuthorizationFailureReason(this, "User no role configured");


                    context.Fail(f);
                }



                return Task.CompletedTask;

            }
            catch(Exception ex)
            {
                return Task.CompletedTask;
            }
            finally
            {
                
            }


        }
    }


    public class TechViewHandler : AuthorizationHandler<TechRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TechRequirement requirement)
        {
            
            var r = requirement.Requirement;
            //context.User.IsInRole(r)

            if (r=="tech" && context.User.IsInRole("xxxpepexxx"))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }


            var pendingCopy = context.PendingRequirements.ToArray();
            
            foreach (TechRequirement req in pendingCopy)
            {
               

                //if (!context.User.HasClaim(c => c.Type == "website"))
                //{
                //    return Task.CompletedTask;
                //}


               
            }

            return Task.CompletedTask;

            //var emailAddress = context.User.FindFirst(c => c.Type == "webSite").Value;

            //if (emailAddress.EndsWith(requirement.CompanyDomain))
            //{
            //    context.Succeed(requirement);

            //}

            //return Task.CompletedTask;
        }
    }
}
