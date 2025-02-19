using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HomeOffice.Core;
using HomeOffice.Data;
using Microsoft.AspNetCore.Mvc;


namespace HomeOffice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AuthController : ControllerBase
    {

        private IUserService _userService;
        // Instanz vom DbContext erstellen
        private readonly AppDbContext _context;
        // zuweisen
        public AuthController(AppDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserModel login)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == login.Username);

            if (user == null)
            {
                return Unauthorized(new { Message = "Benutzername oder Passwort ungültig" });
            }

            // neues Passwort anlegen 
            if (user.Password == "")
            {
                login.Password = PasswordHasher.HashPassword(login.Password);
                user.Password = login.Password;
                _context.SaveChanges();
            }

            bool isPasswordValid = PasswordHasher.VerifyPassword(login.Password, user.Password);

            if (user != null && !isPasswordValid)
            {
                return Unauthorized(new { Message = "Benutzername oder Passwort ungültig" });
            }
            
            _userService.userId = user.Id;
            return Ok(new { Message = $"Login war erfolgreich mit dem User: {user.Username} und der Id: {_userService.userId}" });
        }
    }


}

