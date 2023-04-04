using LoginSystem.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LoginSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleAssignController : ControllerBase
    {
        protected ApiResponse _response;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleAssignController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            this._response = new();

        }
  
        [HttpPost]
        [Authorize(Roles= "SupperAdmin")]
        public async Task<ActionResult<ApiResponse>> AddRole(RoleAssignModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Model is invalid");
                }
                RoleAssignModel role = new RoleAssignModel();
                role.UserName = model.UserName;
                role.Name = model.Name;
                var user = _userManager.FindByNameAsync(role.UserName).Result;
                if (!_roleManager.RoleExistsAsync(model.Name).GetAwaiter().GetResult())
                {
                    return BadRequest("Role does not exist");
                }

                if (user == null)
                    return BadRequest("User not found");
                await _userManager.AddToRoleAsync(user, model.Name);


                _response.Result = user;
                _response.StatusCode = HttpStatusCode.Created;
                return Ok(_response);

            }catch(Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                    = new List<string>() { ex.ToString() };
            }

            return _response;



        }

    }
}
