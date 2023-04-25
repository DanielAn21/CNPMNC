using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace LibManager.Controllers;

[Authorize]
public class BookController : Controller
{
    private readonly ILogger<BookController> _logger;
    private readonly LibDbContext _dbContext;

    public BookController(ILogger<BookController> logger, LibDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index(string search = "")
    {
        var categories = await _dbContext.Categories.ToListAsync();
        ViewBag.categories = categories;

        var books = await _dbContext.Books.Include(x => x.category).
        Where(x => x.title.ToLower().Contains(search.ToLower())).ToListAsync();
        return View(books);

    }
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> Create()
    {
        var categories = await _dbContext.Categories.ToListAsync();
        ViewBag.categories = categories;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([Bind] Book book, string category)
    {
        var categories = await _dbContext.Categories.ToListAsync();
        ViewBag.categories = categories;

        try
        {
            var categoryDb = await _dbContext.Categories.FirstOrDefaultAsync(x => x.id == category);
            if (categoryDb == null) throw new Exception("not found category");

            book.category = categoryDb;
            await _dbContext.Books.AddAsync(book);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        catch (System.Exception ex)
        {
            return Ok(ex.Message);
        }
    }

    [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var categories = await _dbContext.Categories.ToListAsync();
        ViewBag.categories = categories;
        try
        {
            var book = await _dbContext.Books.FirstOrDefaultAsync(x => x.id == id);
            return View(book);
        }
        catch (System.Exception ex)
        {
            return Ok(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit([Bind] Book book, string category)
    {
        var categories = await _dbContext.Categories.ToListAsync();
        ViewBag.categories = categories;
        try
        {

            var bookDb = await _dbContext.Books.FirstOrDefaultAsync(x => x.id == book.id);
            if (bookDb != null)
            {
                var categoryDb = await _dbContext.Categories.FirstOrDefaultAsync(x => x.id == category);
                if (categoryDb != null) bookDb.category = categoryDb;
                _dbContext.Entry(bookDb).CurrentValues.SetValues(book);
                await _dbContext.SaveChangesAsync();
            }
            return View(book);
        }
        catch (System.Exception ex)
        {
            return Ok(ex.Message);
        }
    }


    [HttpPost]
    [Authorize(Roles = "admin")]

    public async Task<IActionResult> delete(string id)
    {
        try
        {
            var book = await _dbContext.Books.FirstOrDefaultAsync(x => x.id == id);
            if (book != null)
            {
                _dbContext.Books.Remove(book);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index");
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
