using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibManager.Models;

namespace LibManager.Controllers;

public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    private readonly LibDbContext _dbContext;

    public UserController(ILogger<UserController> logger, LibDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public IActionResult Index()
    {

        return Ok(_dbContext.Users);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
