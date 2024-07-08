using HomeOffice.Data;
using HomeOffice.Core;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class TimeController : Controller
{
    private readonly AppDbContext _context;

    public TimeController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("start")]
    public IActionResult StartTime([FromBody] int userId)
    {
        var date = DateTime.Now.Date;
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);
        var existingEntry = _context.Time.FirstOrDefault(e => e.Userid == userId && e.Date == date);

        // überprüfen ob der User existiert
        if (user == null)
        {
            return NotFound(new { Message = "User not found" });
        }
        // überprüfen ob ein Eintrag für den jeweilige Tag existiert
        if (existingEntry == null)
        {
            var newEntry = new HomeOfficeTimeModel
            {
                Userid = userId,
                Date = date,
                TotalMinutes = 0
            };
            _context.Time.Add(newEntry);
            _context.SaveChanges();

        }

        // Startzeit speichern
        HttpContext.Session.SetString($"startTime_{userId}", DateTime.Now.ToString());

        return Ok(new { Message = "Zeit wird aufgenommen" });
    }

    [HttpPost("stop")]
    public IActionResult StopTime([FromBody] int userId)
    {
        var date = DateTime.Now.Date;

        var existingEntry = _context.Time
            .FirstOrDefault(t => t.Userid == userId && t.Date == date);

        if (existingEntry != null)
        {
            var startTimeStr = HttpContext.Session.GetString($"startTime_{userId}");
            var startTime = DateTime.Parse(startTimeStr);
            var duration = DateTime.Now.Subtract(startTime).TotalMinutes;

            existingEntry.TotalMinutes += (int)duration;

            // änderung abspeichern
            _context.SaveChanges();

            HttpContext.Session.Remove($"startTime_{userId}");

            return Ok(new { Message = "Zeitaufnahme beendet", TotalMinutes = existingEntry.TotalMinutes });
        }

        return BadRequest(new { Message = "Zeitaufnahme wurde noch nicht gestartet" });
        //Test
        
    }
}
