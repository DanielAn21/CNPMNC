using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace LibManager.Controllers;

[Authorize]
public class TrackingController : Controller
{
    private readonly ILogger<TrackingController> _logger;
    private readonly LibDbContext _dbContext;

    public TrackingController(ILogger<TrackingController> logger, LibDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index()
    {

        return View();

    }
    [HttpPost]
    public async Task<IActionResult> Tracking(string id)
    {

        try
        {
            var track = await _dbContext.Borrowings.Include(x => x.book).Include(x => x.user).FirstOrDefaultAsync(x => x.id == id);
            if (track == null) throw new Exception("not found tracking");
            return View(track);
        }
        catch (System.Exception ex)
        {
            return Ok(ex.Message);
        }
    }


}
