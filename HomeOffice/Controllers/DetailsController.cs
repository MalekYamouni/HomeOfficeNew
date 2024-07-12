using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HomeOffice.Data;
using Microsoft.EntityFrameworkCore;
using HomeOffice.Core;
using System.Security.Claims;

namespace HomeOffice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetailsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUserService _user;

        public DetailsController(AppDbContext context, IUserService user)
        {
            _context = context;
            _user = user;
        }

        [HttpGet("getAll")]
        public ActionResult<IEnumerable<HomeOfficeTimeModel>> GetData()
        {
            var user = _user.userId;

            if (user == null)
            {
                return NotFound("User not found");
            }
            var data = _context.Time.ToList();
            return Ok(data);
        }

    }
}


