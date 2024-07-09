using HomeOffice.Data;
using HomeOffice.Core;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Routing;

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
    public IActionResult StartTime()
    {
        // User von der Session getten
        // HttpContext.Session.SetInt32("userId", userId);

        var date = DateTime.Now.Date;
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var existingEntry = _context.Time.FirstOrDefault(e => e.Userid == userId && e.Date == date);

        // überprüfen ob der User existiert
        if (userId == null)
        {
            return NotFound(new { Message = "User nicht gefunden" });
        }
        // überprüfen ob ein Eintrag für den jeweiligen Tag existiert
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
        _context.SaveChanges();

        // startZeit speichern
        HttpContext.Session.SetString($"startTime_{userId}", DateTime.Now.ToString());

        return Ok(new { Message = "Zeit wird aufgenommen" });
    }

    [HttpPost("stop")]
    public IActionResult StopTime()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var date = DateTime.Now.Date;

        var existingEntry = _context.Time
            .FirstOrDefault(t => t.Userid == userId && t.Date == date);

        var startTimeStr = HttpContext.Session.GetString($"startTime_{userId}");
        var startTime = DateTime.Parse(startTimeStr);
        var duration = DateTime.Now.Subtract(startTime).TotalMinutes;

        if (string.IsNullOrEmpty(startTimeStr))
        {
            return BadRequest(new { Message = "Zeitaufnahme wurde noch nicht gestartet" });

        }

        if (existingEntry != null)
        {
            existingEntry.TotalMinutes += (int)duration;

            // änderung abspeichern
            _context.SaveChanges();

        }

        return Ok(new { Message = "Zeitaufnahme beendet", TotalMinutes = existingEntry.TotalMinutes });
    }
}
