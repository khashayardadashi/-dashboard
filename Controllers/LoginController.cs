using System.Security.Claims;
using cvweb.Services.Intefaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using persian_code.web.dbcontext;
using persian_code.web.ViewModels;

namespace persian_code.web.Controllers;

public class LoginController : Controller
{
    private readonly Applicationdbcontext _dbcontext;
    private readonly IPassswordHash _passswordHash;
    public LoginController(Applicationdbcontext dbcontext , IPassswordHash passswordHash)
    {
        _dbcontext = dbcontext;
        _passswordHash = passswordHash;
    }
    [HttpGet("/Login")]
    public IActionResult Index()
    {
        return View();
    }
    [HttpPost("/Login")]
    public async Task<IActionResult> Index(LoginViewModel Login)
    {
        if (ModelState.IsValid)
        {
            var user = _dbcontext.Users.SingleOrDefault(u => u.Email == Login.Email);
            if (user != null)
            {
                if (user.Password == _passswordHash.HashPassword(Login.Password))
                {
                    if (user.IsActive)
                    {
                        var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.Name , user.Username),
                           
                        };

                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        var properties = new AuthenticationProperties()
                        {
                            IsPersistent = Login.Remember
                        };
                        await HttpContext.SignInAsync(principal, properties);
                        return RedirectToAction("Index" , "Account");
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "حساب کاربری شما فعال نشده است");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "کاربری با مشخصات وارد شده یافت نشد");
                }
            }
            else
            {
                ModelState.AddModelError("Email", "کاربری با مشخصات وارد شده یافت نشد");
            }
        }

        return View();
    }
}