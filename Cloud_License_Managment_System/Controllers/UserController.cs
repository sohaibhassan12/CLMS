using Cloud_License_Managment_System.Models;
using Cloud_License_Managment_System.Models.VM_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cloud_License_Managment_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly CLMS_DbContext _DbContext;

        public UserController(IConfiguration configuration, CLMS_DbContext dbContext)
        {
            _configuration = configuration;
            _DbContext = dbContext;
        }

        [HttpPost]
        [Route("SaveUser")]
        public IActionResult SaveUser(Users model)
        {
            if(ModelState.IsValid)
            {
                _DbContext.Users.Add(model);
                _DbContext.SaveChanges();
            }
            return Ok(model);
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login(VMLogin model)
        {


            if (IsValidUser(model.Email, model.Password))
            {
                
                return Ok("User LogedIn successfully!");
            }

            return Unauthorized();
        }


        private bool IsValidUser(string username, string password)
        {
            var user = _DbContext.Users
            .SingleOrDefault(u => u.Email == username && u.Password == password);
            return user != null;
        }

    }
    
}
