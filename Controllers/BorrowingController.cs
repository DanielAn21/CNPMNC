using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibManager.Models;
using Microsoft.EntityFrameworkCore;

namespace LibManager.Controllers;

public class BorrowingController : Controller
{
    private readonly ILogger<BorrowingController> _logger;
    private readonly LibDbContext _dbContext;

    public BorrowingController(ILogger<BorrowingController> logger, LibDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index()
    {
        var borrowings = await _dbContext.Borrowings.Include(x => x.book).Include(x => x.user).ToListAsync();
        return View(borrowings);

    }

    public IActionResult Create()
    {
        ShowAllUsersAndBooks();
        return View();
    }

    public async void ShowAllUsersAndBooks()
    {
        var books = await _dbContext.Books.ToListAsync();
        var users = await _dbContext.Users.ToListAsync();
        ViewBag.books = books;
        ViewBag.users = users;

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
            return RedirectToAction("Index");
        }
        catch (System.Exception ex)
        {
            return Ok(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> edit(string id)
    {
        try
        {
            var borrowing = await _dbContext.Borrowings.Include(x => x.book).Include(x => x.user).FirstOrDefaultAsync(x => x.id == id);
            return View(borrowing);
        }
        catch (System.Exception ex)
        {
            return Ok(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> edit([Bind] Borrowing borrowing, string userId, string bookId)
    {
        try
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.id == userId);
            var book = await _dbContext.Books.FirstOrDefaultAsync(x => x.id == bookId);
            var borrowingDb = await _dbContext.Borrowings.FirstOrDefaultAsync(x => x.id == borrowing.id);
            if (borrowingDb == null || user == null || book == null) throw new Exception(
                "not found user or book"
            );
            borrowing.user = user;
            borrowing.book = book;


            if (borrowingDb != null)
            {
                _dbContext.Entry(borrowingDb).CurrentValues.SetValues(borrowing);
                await _dbContext.SaveChangesAsync();
            }
            return View(borrowing);
        }
        catch (System.Exception ex)
        {
            return Ok(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> delete(string id)
    {
        try
        {
            var borrowing = await _dbContext.Borrowings.FirstOrDefaultAsync(x => x.id == id);
            if (borrowing != null)
            {
                _dbContext.Borrowings.Remove(borrowing);
                await _dbContext.SaveChangesAsync();

            }
            return RedirectToAction("Index");
        }
        catch (System.Exception ex)
        {
            return RedirectToAction("Index");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
