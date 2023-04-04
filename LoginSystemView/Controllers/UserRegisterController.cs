using LoginSystemView.Models;
using LoginSystemView.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NToastNotify;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Web.Http;

namespace LoginSystemView.Controllers
{
    [Authorize]
    public class UserRegisterController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _options;
        private readonly IConfiguration _configuration;
        private readonly IToastNotification _toastNotification;
        private readonly IUserRoles _userRoles;


        public UserRegisterController(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IToastNotification toastNotification,
            IUserRoles userRoles


            )
        {
            _httpClientFactory = httpClientFactory;
            _options = new JsonSerializerOptions();
            _configuration = configuration;
            _toastNotification = toastNotification;
            _userRoles = userRoles;


        }

        public async Task<IActionResult> Index()
        {
            var baseUrl = _configuration.GetSection("Url")["baseUrl"];

            using (var httpClient = new HttpClient())
            {
                var response = httpClient.GetAsync(baseUrl + "/api/UserRegister/GetData?userName=shyam123456").Result.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<User>(response.Result);
                return View();
            }

        }
        //Navigate to the CreateUser page 
        public ActionResult CreateUser()
        {
            User model = new User();
            return View();
        }



        public async Task<IActionResult> Create(User UserData)
        {
            var baseUrl = _configuration.GetSection("Url")["baseUrl"];
            var client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            User user = new User();
            user.UserName = UserData.UserName;
            user.PhoneNumber = UserData.PhoneNumber;
            user.FirstName = UserData.FirstName;
            user.LastName = UserData.LastName;
            user.Email = UserData.Email;
            user.PasswordHash = UserData.PasswordHash;
            HttpContent body = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = client.PostAsync("/api/UserRegister", body).Result;
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                return RedirectToAction("Index");

            }
            return RedirectToAction("CreateUser");




        }

        //Navigate to the login page 
        public ActionResult LoginPage()
        {
            return View("UserLogin");
        }


        //for Login post request 

        public async Task<IActionResult> UserLogin(UserLogin UserCredential)
        {
            var baseUrl = _configuration.GetSection("Url")["baseUrl"];
            var client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            UserLogin user = new UserLogin();
            user.UserName = UserCredential.UserName;
            user.Password = UserCredential.Password;

            HttpContent body = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = client.PostAsync("/api/UserRegister/Login", body).Result;
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                ApiResponse data = JsonConvert.DeserializeObject<ApiResponse>(content);

                var claims = new List<Claim>
                {
                     new Claim(ClaimTypes.Name, user.UserName),
                     new Claim(type: "id", value: data.userId),
                     new Claim(ClaimTypes.Email, data.userEmail),
                     new Claim(ClaimTypes.NameIdentifier, data.userId)






                };
                List<UserRolesModel> list = new();
                //var response = await _userRoles.GetAllAsync<Response>();
                var anotherResponse = await _userRoles.GetAsync<Response>(UserCredential.UserName);
               
                if (anotherResponse!=null)
                {
                    
                    var role = JsonConvert.DeserializeObject<UserRolesModel>(Convert.ToString(anotherResponse.Result));

                   claims.Add(new Claim(ClaimTypes.Role, role.Role));
                    


                }




                //add rolesmif any 

                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                   );
                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20),
                    IsPersistent = true,


                };
                await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                if (!string.IsNullOrEmpty(content))
                {
                    var objDeserializeObject = JsonConvert.DeserializeObject<ApiResponse>(content);

                    if (objDeserializeObject != null)
                    {
                        Console.WriteLine(objDeserializeObject.token.ToString());

                    }
                }
                _toastNotification.AddSuccessToastMessage("Login Successfully");

                //Add HttpContext
                HttpContext.Session.SetString("token", data.token);

                return RedirectToAction("Index", "Dashboard");

            }



            _toastNotification.AddSuccessToastMessage("Login faield");
            return RedirectToAction("Index");
        }
    }
}
