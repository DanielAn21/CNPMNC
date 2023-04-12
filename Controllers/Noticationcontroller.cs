using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibManager.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace LibManager.Controllers;

[Authorize]
public class NoticationController : Controller
{
    private readonly ILogger<NoticationController> _logger;
    private readonly LibDbContext _dbContext;

    public NoticationController(ILogger<NoticationController> logger, LibDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index()
    {
        var email = User.FindFirst(ClaimTypes.Name)?.Value;
        var notications = await _dbContext.Notications.Include(x => x.receiver).Include(x => x.sender).Where(x => x.sender.email.Equals(email) || x.receiver.email.Equals(email)).ToListAsync();
        return View(notications);
    }

    public async Task<IActionResult> Create()
    {

        return View();
    }

    public async Task<IActionResult> Details(string id)
    {
        var notication = await _dbContext.Notications.Include(x => x.sender).Include(x => x.receiver).FirstOrDefaultAsync(x => x.id == id);
        return View(notication);
    }

    [HttpPost]
    public async Task<IActionResult> Create([Bind] Notication notication, string receiver)
    {


        try
        {
            var emailUser = User.FindFirst(ClaimTypes.Name)?.Value;
            var sender = await _dbContext.Users.FirstOrDefaultAsync(x => x.email == emailUser);
            var receiverUser = await _dbContext.Users.FirstOrDefaultAsync(x => x.email == receiver);
            if (sender == null || receiverUser == null) throw new Exception("not found sender or receiver");
            notication.sender = sender;
            notication.receiver = receiverUser;

            await _dbContext.Notications.AddAsync(notication);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
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
            var noti = await _dbContext.Notications.FirstOrDefaultAsync(x => x.id == id);
            if (noti != null)
            {
                _dbContext.Notications.Remove(noti);
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
