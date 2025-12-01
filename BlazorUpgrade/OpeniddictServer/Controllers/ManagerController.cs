

using CalibrationSaaS.Domain.Aggregates.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpeniddictServer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IdentityProvider.Quickstart
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ManagerController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        // GET: api/<ManagerController>
        [HttpGet("All")]
        public async Task<IEnumerable<User>> GetAll()
        {
            var alice = await GetUsersAsync();

            List<User> us = new List<User>();

            foreach (var item in alice)
            {
                User user = new User();

                user.UserName = item.UserName;
                user.Email = item.Email;
                user.IdentityID = item.Id;
                var user1 = await Get(item.UserName);
                if (user1 != null)
                {
                    user.Name = user1.Name;
                    user.LastName = user1.LastName;
                }

                us.Add(user);
            }

            return us;

        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersAsync()
        {
            //using (var context = new YourContext())
            //{
            return await _userManager.Users.ToListAsync();
            //}
        }

        [HttpGet("DEL/{id}")]
        public async Task<User> DEL(string id)
        {

            try
            {

                var alice = await _userManager.FindByNameAsync(id);

                if (alice != null)
                {
                    var cc = await _userManager.GetClaimsAsync(alice);

                    var result = await _userManager.RemoveClaimsAsync(alice, cc);
                    var result2 = await _userManager.DeleteAsync(alice);
                }


                var user = new User();


                return user;
            }
            catch (Exception ex)
            {

                return null;
            }

        }

        [HttpGet("reset/{id}")]
        public async Task<IActionResult> GetReset(string id)
        {
            try
            {
                var a = await Reset2(id);

                return Ok(a);
            }
            catch (Exception ex)
            {
                return StatusCode(501);
            }


        }

        [HttpGet("disable/{id}")]
        public async Task<IActionResult> GetDisable(string id)
        {

            try
            {
                var a = await Disable(id);

                return Ok(a);
            }
            catch (Exception ex)
            {

                return StatusCode(501);

            }



        }


        public async Task<bool> Disable(string id)
        {
            var user = await Get(id);

            if (user != null)
            {

                var claim = user.RolesList.Where(x => x.Name.ToLower() == "disable").FirstOrDefault();

                var claim2 = new Claim("Role", "disable");

                if (claim != null)
                {
                    await _userManager.RemoveClaimAsync(user.UserIdentity, claim2);


                }
                else
                {
                    await _userManager.AddClaimAsync(user.UserIdentity, claim2);

                }
                return true;
            }
            return false;
        }






        public async Task<bool> Reset2(string id)
        {
            var user = await Get(id);

            if (user != null)
            {

                var claim = user.RolesList.Where(x => x.Name.ToLower() == "reset").FirstOrDefault();

                var claim2 = new Claim("Role", "reset");

                if (claim != null)
                {
                    await _userManager.RemoveClaimAsync(user.UserIdentity, claim2);


                }
                else
                {
                    await _userManager.AddClaimAsync(user.UserIdentity, claim2);

                }
                return true;
            }
            return false;
        }




        // GET api/<ManagerController>/5
        [HttpGet("{id}")]
        public async Task<User> Get(string id)
        {
            var alice = await _userManager.FindByNameAsync(id);

            if (alice == null)
            {
                alice = await _userManager.FindByIdAsync(id);
            }

            if (alice == null)
            {
                return new User();
            }


            var user = new User();

            user.UserIdentity = alice;
            user.UserName = alice.UserName;
            user.Email = alice.Email;
            user.IdentityID = alice.Id;

            var c = await _userManager.GetClaimsAsync(alice);
            user.RolesList = new List<Rol>();
            foreach (var cl in c)
            {
                if (cl.Type == "Role")
                {
                    Rol r = new Rol();
                    r.Name = cl.Value;
                    user.RolesList.Add(r);
                    user.Roles = user.Roles + cl.Value + ",";
                    //user.Roles = r.Name + ",";
                }

                if (cl.Type == "Name")
                {
                    user.Name = cl.Value;
                }

                if (cl.Type == "FamilyName")
                {
                    user.LastName = cl.Value;
                }

                //if (cl.Type == JwtClaimTypes.)
                //{
                //    user.LastName = cl.Value;
                //}
            }


            return user;
        }

        // POST api/<ManagerController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string value1)
        {
            User value = Newtonsoft.Json.JsonConvert.DeserializeObject<User>(value1);
            try
            {
                var alice = await _userManager.FindByNameAsync(value.UserName);

                if (alice == null)
                {
                    //var user = new User();
                    //user.Name = alice.UserName;
                    //user.Email = alice.Email;
                    //return user;

                    alice = new ApplicationUser
                    {
                        UserName = value.UserName,
                        Email = value.Email,
                        EmailConfirmed = true,
                    };
                    var result = await _userManager.CreateAsync(alice, value.PassWord);

                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                    else
                    {

                        result = await _userManager.AddClaimsAsync(alice, new Claim[]{
                            new Claim("Name", value.UserName),
                            //new Claim(JwtClaimTypes.GivenName, value.UserName),
                            //new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            //new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                             //new Claim("Role", "admin"),
                             new Claim("ApplicationRole","Owner"),
                            //new Claim("ClaimTest","test")
                        });
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        if (!string.IsNullOrEmpty(value.Roles))
                        {
                            foreach (var rol in value.Roles.Split(','))
                            {
                                if (!string.IsNullOrEmpty(rol))
                                {
                                    result = await _userManager.AddClaimsAsync(alice, new Claim[]
                               {
                                            new Claim("Role", rol.Trim()),
                                   //  new Claim("ApplicationRole","Owner"),
                                   //new Claim("ClaimTest","test")
                                    });
                                    if (!result.Succeeded)
                                    {
                                        throw new Exception(result.Errors.First().Description);
                                    }
                                }

                            }
                        }


                        var result2 = await _userManager.FindByNameAsync(alice.UserName);
                        value.IdentityID = result2.Id;
                    }

                }
                else
                {
                    value.IdentityID = alice.Id;

                    //alice = new ApplicationUser
                    //{
                    //    UserName = value.UserName,
                    //    Email = value.Email,
                    //    EmailConfirmed = true,
                    //};
                    if (!string.IsNullOrEmpty(value.PassWord) && !string.IsNullOrEmpty(value.OldPassWord))
                    {


                        var cc11 = await _userManager.ChangePasswordAsync(alice, value.OldPassWord, value.PassWord);
                    }

                    var cc = await _userManager.GetClaimsAsync(alice);


                    if (value.RolesList == null)
                    {
                        value.RolesList = new List<Rol>();

                        if (!string.IsNullOrEmpty(value.Roles))
                        {
                            var sss = value.Roles.Split(',', StringSplitOptions.RemoveEmptyEntries);

                            foreach (var item2 in sss)
                            {
                                var r = new Rol();
                                r.Name = item2.Trim();

                                value.RolesList.Add(r);

                            }
                        }

                    }



                    var result0 = cc.Where(p => value.RolesList.All(p2 => p2.Name.ToLower() != p.Value.ToLower()));
                    if (result0 != null && result0.Count() > 0)
                    {
                        foreach (var tt in result0)
                        {
                            if (tt.Type == "Role")
                            {
                                await _userManager.RemoveClaimAsync(alice, tt);
                            }

                        }

                    }



                    foreach (var rol in value.RolesList)
                    {
                        var ccc = cc.Where(x => x.Type == "Role" && x.Value == rol.Name).FirstOrDefault();
                        if (ccc == null)
                        {
                            var result = await _userManager.AddClaimsAsync(alice, new Claim[]
                       {
                             new Claim("Role", rol.Name),
                           //  new Claim("ApplicationRole","Owner"),
                           //new Claim("ClaimTest","test")
                       });
                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }
                        }

                    }


                }

                return Ok(value);
            }
            catch (Exception ex)
            {
                value.Message = ex.Message;
                return StatusCode(500, value);
            }
        }

        // PUT api/<ManagerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ManagerController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {

                var alice = await _userManager.FindByNameAsync(id);
                var cc = await _userManager.GetClaimsAsync(alice);

                var result = await _userManager.RemoveClaimsAsync(alice, cc);
                var result2 = await _userManager.DeleteAsync(alice);

                var user = new User();


                return Ok(user);
            }
            catch (Exception ex)
            {

                return StatusCode(500);
            }

        }


        // DELETE api/<ManagerController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRol(string id)
        {
            try
            {
                //var users = _userManager.Users.ToList();

                var claim = new Claim("Role", id);

                var users = await _userManager.GetUsersForClaimAsync(claim);



                foreach (var item in users)
                {

                    await _userManager.RemoveClaimAsync(item, claim);
                }



                return Ok();
            }
            catch (Exception ex)
            {

                return StatusCode(500);
            }

        }

        public async Task<string> CallApi()
        {

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var content = await client.GetStringAsync("https://localhost:6001/identity");

            //ViewBag.Json = JArray.Parse(content).ToString();
            return content;
        }

    }
}