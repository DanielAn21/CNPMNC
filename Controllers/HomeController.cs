using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace LibManager.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<BorrowingController> _logger;
    private readonly LibDbContext _dbContext;

    public HomeController(ILogger<BorrowingController> logger, LibDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }


    public async Task<IActionResult> Index()
    {
        var books = await _dbContext.Books.Include(x => x.borrowings).OrderByDescending(x => x.borrowings.Count()).ToListAsync();
        var total = _dbContext.Borrowings.Sum(x => x.fineAmount);
        var borrowings = await _dbContext.Borrowings.Include(x => x.user).Where(x => DateTime.Compare(DateTime.Now, x.returnDate) == 1).ToListAsync();
        ViewBag.books = books;
        ViewBag.total = total;
        ViewBag.borrowings = borrowings;
        return View();
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
