using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibManager.Models;
using Microsoft.EntityFrameworkCore;
using LibManager.utils;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace LibManager.Controllers;

public class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;
    private readonly LibDbContext _dbContext;

    public AuthController(ILogger<AuthController> logger, LibDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public IActionResult Login(string? returnUrl)
    {
        returnUrl = returnUrl ?? "/";
        ViewBag.returnUrl = returnUrl;
        if (User?.Identity?.IsAuthenticated == true)
            return Redirect(returnUrl);
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password, string returnUrl)
    {
        returnUrl = returnUrl ?? "/";
        ViewBag.returnUrl = returnUrl;
        try
        {
            User user = validateUser(email, password);
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.email),
                    new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
                    new Claim(ClaimTypes.Role, user.role.ToString()),
                };

            _logger.LogInformation(user.role.ToString());

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
            };

            await HttpContext.SignInAsync(
                scheme: "libmanager",
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            return Redirect(returnUrl);
        }
        catch (Exception ex)
        {
            ViewBag.errorMessage = ex.Message;
            return View();
        }
    }

    public async Task<IActionResult> Logout(string? returnUrl)
    {
        returnUrl = returnUrl ?? "/auth/login";

        await HttpContext.SignOutAsync(
        scheme: "libmanager");

        return Redirect(returnUrl);
    }

    public async Task<IActionResult> Forbidden()
    {
        await Task.CompletedTask;
        return View();
    }

    private User validateUser(string email, string password)
    {
        try
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.email == email);
            if (user == null)
                throw new Exception("email is incorrect ");
            if (!MD5Password.ComparePassword(password, user.hashPassword))
                throw new Exception("password is incorrect");
            return user;
        }
        catch (System.Exception ex)
        {

            throw new Exception(ex.Message);
        }
    }

}
