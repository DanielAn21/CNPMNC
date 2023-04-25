using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace LibManager.Controllers;

[Authorize]
public class RegisterBookController : Controller
{
    private readonly ILogger<RegisterBookController> _logger;
    private readonly LibDbContext _dbContext;

    public RegisterBookController(ILogger<RegisterBookController> logger, LibDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }



    public async Task<IActionResult> Create()
    {
        var books = await _dbContext.Books.ToListAsync();
        var users = await _dbContext.Users.ToListAsync();
        ViewBag.books = books;
        ViewBag.users = users;
        return View();
    }



    [HttpPost]
    public async Task<IActionResult> Create([Bind] Borrowing borrowing, string userId, string bookId)
    {

        try
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.id == userId);
            var book = await _dbContext.Books.FirstOrDefaultAsync(x => x.id == bookId);
            if (user == null || book == null) throw new Exception(
                "not found user or book"
            );

            borrowing.user = user;
            borrowing.book = book;
            await _dbContext.Borrowings.AddAsync(borrowing);
            await _dbContext.SaveChangesAsync();
            return Redirect("/");
        }
        catch (System.Exception ex)
        {
            return Ok(ex.Message);
        }
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
