using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HomeOffice.Core;
using HomeOffice.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HomeOffice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AuthController : ControllerBase
    {

        // Instanz vom DbContext erstellen
        private readonly AppDbContext _context;

        // zuweisen
        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserModel login)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == login.Username);

            if (user == null)
            {
                return Unauthorized(new { Message = "Benutzername oder Passwort ungültig" });
            }

            if (user.Password == ""){
                login.Password = PasswordHasher.HashPassword(login.Password);
                user.Password = login.Password;
                _context.SaveChanges();
            }

            bool isPasswordValid = PasswordHasher.VerifyPassword(login.Password, user.Password);
            if (user != null && !isPasswordValid)
            {
                return Unauthorized(new { Message = "Benutzername oder Passwort ungültig" });
            }

            return Ok(new { Message = "Login war erfolgreich" });
        }
    }


}

