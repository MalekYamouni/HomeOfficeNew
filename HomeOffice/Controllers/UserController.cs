using HomeOffice.Core;
using HomeOffice.Data;
using Microsoft.AspNetCore.Mvc;

namespace HomeOffice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UserController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public UserController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public IEnumerable<UserModel> GetUsers()
        {
            return _appDbContext.Users.ToList();
        }
    }
}





