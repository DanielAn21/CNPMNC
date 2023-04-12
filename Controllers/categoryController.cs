using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace LibManager.Controllers;

[Authorize]
public class CategoryController : Controller
{
    private readonly ILogger<CategoryController> _logger;
    private readonly LibDbContext _dbContext;

    public CategoryController(ILogger<CategoryController> logger, LibDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _dbContext.Categories.ToListAsync();
        return View(categories);

    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([Bind] Category category)
    {
        try
        {
            var categoryNew = await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        catch (System.Exception ex)
        {
            return View(category);
        }
    }

    [HttpGet]
    public async Task<IActionResult> edit(string id)
    {
        try
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(x => x.id == id);
            return View(category);
        }
        catch (System.Exception ex)
        {
            return Ok(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> edit([Bind] Category category)
    {
        try
        {
            var categoryDb = await _dbContext.Categories.FirstOrDefaultAsync(x => x.id == category.id);
            if (categoryDb != null)
            {
                _dbContext.Entry(categoryDb).CurrentValues.SetValues(category);
                await _dbContext.SaveChangesAsync();
            }
            return View(category);
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
            var category = await _dbContext.Categories.FirstOrDefaultAsync(x => x.id == id);
            if (category != null)
            {
                _dbContext.Categories.Remove(category);
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
