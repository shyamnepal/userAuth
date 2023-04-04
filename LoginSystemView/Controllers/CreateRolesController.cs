using LoginSystemView.Models;
using LoginSystemView.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace LoginSystemView.Controllers
{
    public class CreateRolesController : Controller
    {
        private  readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRoles _userRole;
        public CreateRolesController(
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IUserRoles userRole
            )
           
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _userRole = userRole;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var username = _httpContextAccessor.HttpContext.User.Identity.Name;
            var baseUrl = _configuration.GetSection("Url")["baseUrl"];
            ViewData["userData"] = new User()
            {
                UserName = username,


            };
            if (User.IsInRole("SupperAdmin"))
            {
                ViewBag.UserRole = "SupperAdmin";
            }
            else if (User.IsInRole("User"))
            {
                ViewBag.UserRole = "User";
            }
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token").ToString());
            var response = httpClient.GetAsync(baseUrl + $"/api/Profile/GetUserProfile?UserId={username}").Result;
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();


                var data = JsonConvert.DeserializeObject<userProfileDto>(content);
                ViewData["profileData"] = new UserProfile()
                {
                    imgurl = data.imgurl
                };
            }


            return View();
        }
        [Authorize]
        public async Task<IActionResult> AddRoles(AddRoles role)

        { 
            AddRoles userRole = new AddRoles();
            userRole.Name = role.Name;

            var response = await _userRole.CreateAsync<Response>(role);
            if (response!=null)
            {
                //var content = await response.Content.ReadAsStringAsync();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
