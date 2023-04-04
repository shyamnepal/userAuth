using LoginSystem.Data;
using LoginSystem.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace LoginSystem.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<User> _userManager;
        public ProfileController(AppDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<User> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }
        [HttpPost("AddProfile")]
        public async Task<ActionResult<ApiResponse>> addProfile([FromForm]UserProfileDto profile)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    
                    return BadRequest("Model is not valid");
                }
                    

                string uniqueFilename = UploadedFile(profile);
                UserProfile userProfile = new UserProfile()
                {
                    City = profile.City,
                    imgurl = uniqueFilename,
                    UserId = profile.UserId,
                    Address = profile.Address,
                    Age=profile.Age,
                    Education=profile.Education,
                    Gender=profile.gender
                };

                _context.UserProfiles.Add(userProfile);
                await _context.SaveChangesAsync();

               

                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

       


        [HttpGet("GetUserProfile")]
        public async Task<ActionResult<ApiResponse>> GetUserProfile(string UserId)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(UserId);
                var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == user.Id);
                if (userProfile != null)
                {

                   
                    return Ok(userProfile);

                }
                
                return BadRequest("User Not found");

            }catch(Exception ex)
            {
               
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut("EditUser")]
        public async Task<ActionResult<ApiResponse>> EditUserProfile([FromForm] UserProfileDto profile)
        {
           if (!ModelState.IsValid)
            {
               
                return BadRequest("Model is not valid");
            }
               

           var user= await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == profile.UserId);
            if (profile != null && user!=null)
            {

                string uniqueFilename = UploadedFile(profile);
                user.imgurl = uniqueFilename;
                user.City = profile.City;
                user.UserId = profile.UserId;
                user.Address = profile.Address;
                user.Age = profile.Age;
                user.Education = profile.Education;
                user.Gender = profile.gender;

                DeleteImage(user.imgurl);

                _context.Entry(user).State = EntityState.Modified;
                 _context.SaveChanges();

               
                

                return Ok();
            }
           

            return BadRequest("Failed to update the profile");
        }

        private string UploadedFile(UserProfileDto model)
        {
            string uniqueFileName = null;
            if (model.Image != null)
            {
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        private void DeleteImage(string fileName)
        {
            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "image", fileName);
            if (System.IO.File.Exists(imagePath))
            {
                try
                {
                    System.IO.File.Delete(imagePath);
                }
                catch(Exception ex)
                {
                    throw new Exception( ex.Message);
                }
            }
            


        }

    }
}
