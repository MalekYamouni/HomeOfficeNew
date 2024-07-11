using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HomeOffice.Data;
using Microsoft.EntityFrameworkCore;

namespace HomeOffice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DetailsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DetailsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("userId/{userId}")]
        public IActionResult GetHomeOfficeData(int userId)
        {
            // var data = await _context.Time
            //     .FirstOrDefaultAsync(t => t.Userid == userId );

            var data = _context.Time.FirstOrDefault(t=> t.UserId == userId);
            // if (data == null)
            // {
            //     return NotFound(new { message = "No data found for this day" });
            // }
            return Ok(data);
        }
    }
}
