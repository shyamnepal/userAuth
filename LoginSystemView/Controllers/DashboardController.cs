using LoginSystemView.Models;
using LoginSystemView.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace LoginSystemView.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IUsersPofiles _userProfile;
        public DashboardController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            IUsersPofiles usersProfile
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _userProfile = usersProfile;
        }
        public async Task<IActionResult> Index()
        
        {
            var username = _httpContextAccessor.HttpContext.User.Identity.Name;
            if (username != null)
            {
                var baseUrl = _configuration.GetSection("Url")["baseUrl"];

                using (var httpClient = new HttpClient())
                {
                   // var response = httpClient.GetAsync(baseUrl + $"/api/UserRegister/GetData?userName={username}").Result.Content.ReadAsStringAsync();
                    //var data = JsonConvert.DeserializeObject<User>(response.Result);
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
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("token").ToString());
                    var response = httpClient.GetAsync(baseUrl + $"/api/Profile/GetUserProfile?UserId={username}").Result;

                   // var response = await _userProfile.GetUserProfileAsync<Response>(username);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();


                        var data = JsonConvert.DeserializeObject<userProfileDto>(content);
                        ViewData["profileData"] = new UserProfile()
                        {
                            imgurl = data.imgurl
                        };
                       

                        return View("Dashboard");
                        
                    }
                   

                    return View("Dashboard");
                }

               
            }
            return RedirectToAction("LoginPage", "UserRegister");
           // Console.Write(username);
           
            

        }
    }
}
