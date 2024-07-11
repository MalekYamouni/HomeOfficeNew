using HomeOffice.Data;
using HomeOffice.Core;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Routing;
using SQLitePCL;
using HomeOffice.Controllers;
using HomeOffice.Services;

[ApiController]
[Route("api/[controller]")]
public class TimeController : Controller
{
    private readonly AppDbContext _context;
    public IUserService _userService;
    public ITimeService _timeService;
    public int userId;

    public TimeController(AppDbContext context, ITimeService timeService, IUserService userService)
    {
        _context = context;
        _userService = userService;
        _timeService = timeService;
    }

    [HttpPost("start")]
    public IActionResult StartTime()
    {
        _timeService.Start();
        var date = DateTime.Now.Date;
        var existingEntry = _context.Time.FirstOrDefault(e => e.Userid == _userService.userId && e.Date == date);

        // überprüfen ob der User existiert
        if (_userService.userId == null)
        {
            return NotFound(new { Message = "User nicht gefunden" });
        }
        // überprüfen ob ein Eintrag für den jeweiligen Tag existiert
        if (existingEntry == null)
        {
            var newEntry = new HomeOfficeTimeModel
            {
                Userid = _userService.userId,
                Date = date,
                TotalMinutes = 0
            };
            _context.Time.Add(newEntry);
            _context.SaveChanges();
        }
        return Ok(new { Message = $"Zeit wird aufgenommen" });
    }

    [HttpPost("stop")]
    public IActionResult StopTime()
    {
        _timeService.Stop();
        int duration = _timeService.GetElapsedMinutes();
        var date = DateTime.Now.Date;
        var existingEntry = _context.Time
            .FirstOrDefault(t => t.Userid == _userService.userId && t.Date == date);
        var user = _context.Users.FirstOrDefault(u => u.Id == _userService.userId);

        if (existingEntry != null)
        {
            existingEntry.TotalMinutes += (int)duration;
            _context.SaveChanges();
        }
        duration = 0;
        return Ok(new { Message = $"Zeitaufnahme beendet vom User : {user.Username}", TotalMinutes = existingEntry.TotalMinutes });
    }
}
