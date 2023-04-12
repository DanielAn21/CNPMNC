using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibManager.Models;
using Microsoft.EntityFrameworkCore;
using LibManager.utils;

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

    public async Task<IActionResult> Index()
    {
        var accounts = await _dbContext.Users.ToListAsync();
        return View(accounts);

    }

    public async Task<IActionResult> Create()
    {

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([Bind] User user)
    {
        try
        {
            user.hashPassword = MD5Password.HashPass(user.hashPassword);
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (System.Exception ex)
        {
            return Ok(ex.Message);
        }
    }
    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {

        try
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.id == id);
            return View(user);
        }
        catch (System.Exception ex)
        {
            return Ok(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit([Bind] User user)
    {
        try
        {

            var userDb = await _dbContext.Users.FirstOrDefaultAsync(x => x.id == user.id);
            if (userDb != null)
            {
                user.hashPassword = user.hashPassword == null
                ? userDb.hashPassword : MD5Password.HashPass(user.hashPassword);
                _dbContext.Entry(userDb).CurrentValues.SetValues(user);
                await _dbContext.SaveChangesAsync();
            }
            return View(user);
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
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.id == id);

            if (user != null)
            {
                _dbContext.Users.Remove(user);
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
