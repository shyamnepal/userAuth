using LoginSystemView.Models;
using LoginSystemView.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using System.Web.Http;

namespace LoginSystemView.Controllers
{
    [Authorize]
    public class RoleAssignController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRoles _userRole;
     

        public RoleAssignController(
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IUserRoles userRole
            
            )
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _userRole = userRole;
        }
        public async Task<IActionResult> Index()
        {
            
                var username = _httpContextAccessor.HttpContext.User.Identity.Name;
                ViewData["userData"] = new User()
                {
                    UserName = username,


                };

                //check the user is admin or user

                if (User.IsInRole("SupperAdmin"))
                {
                    ViewBag.UserRole = "SupperAdmin";
                }
                else if (User.IsInRole("User"))
                {
                    ViewBag.UserRole = "User";
                }

                var baseUrl = _configuration.GetSection("Url")["baseUrl"];
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token").ToString());
                var responseProfile = httpClient.GetAsync(baseUrl + $"/api/Profile/GetUserProfile?UserId={username}").Result;
                if (responseProfile.IsSuccessStatusCode)
                {
                    var content = await responseProfile.Content.ReadAsStringAsync();


                    var data = JsonConvert.DeserializeObject<userProfileDto>(content);
                    ViewData["profileData"] = new UserProfile()
                    {
                        imgurl = data.imgurl
                    };
                }


                var response = await _userRole.GetOnlyRoles<Response>();
                if (response != null)
                {
                    var data = JsonConvert.DeserializeObject<List<IdentityRole>>(Convert.ToString(response.Result));
                    ViewBag.roles = data.ToList();

                    return View();
                }
            
           

            return View();
               
            


          
        }

        public async Task<IActionResult> RoleAssignToUser(RoleAssignModel model)

        {

          
            RoleAssignModel userRole = new RoleAssignModel();
            userRole.UserName = model.UserName;
            userRole.Name = model.Name;


            //HttpContent body = new StringContent(JsonConvert.SerializeObject(userRole), Encoding.UTF8, "application/json");
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token").ToString());
            //var response = client.PostAsync("/api/RoleAssign", body).Result;
            var response = await _userRole.AssignRoles<Response>(model);


            if (response!=null)
            {
                //var content = await response.Content.ReadAsStringAsync();

                return RedirectToAction("Index");

            }

            return RedirectToAction("Index");
        }
        
    }
}
