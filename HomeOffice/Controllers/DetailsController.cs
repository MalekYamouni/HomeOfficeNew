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
            var data = _context.Time.ToList();
            return Ok(data);
        }
        // [HttpGet("getByDate")]
        // public IActionResult GetByDate(DateTime date)
        // {
        //     var entries = _context.Time
        //         .Where(t => t.Userid == _user.userId && t.Date == date)
        //         .Select(t => new
        //         {
        //             t.Date,
        //             t.TotalMinutes
        //         })
        //         .ToList();

        //     if (!entries.Any())
        //     {
        //         return NotFound(new { Message = "Keine Einträge gefunden" });
        //     }

        //     return Ok(entries);
        // }
    }
}


