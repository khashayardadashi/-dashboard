using System.Security.Claims;
using cvweb.Services.Intefaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
using persian_code.web.dbcontext;
using persian_code.web.Entities;
using persian_code.web.ViewModels;

namespace persian_code.web.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly Applicationdbcontext _dbcontext;
    private readonly IPassswordHash _passswordHash;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public AccountController(Applicationdbcontext dbcontext, IPassswordHash passswordHash,
        IWebHostEnvironment webHostEnvironment )
    {
        _dbcontext = dbcontext;
        _passswordHash = passswordHash;
        _webHostEnvironment = webHostEnvironment;

    }

    [Route("/Account")]
    public IActionResult Index()
    {
        var UserId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        var CurrentUser = _dbcontext.Users.SingleOrDefault(u => u.Id == UserId);
        var Rank = _dbcontext.Users.OrderByDescending(u => u.Score).ToList().IndexOf(CurrentUser) + 1;
        ViewData["Rank"] = Rank;
        if (CurrentUser.Score <= 500)
        {
            ViewData["state"] = "لیگ برنز";
        }
        else if (CurrentUser.Score <= 1000 && CurrentUser.Score >= 500)
        {
            ViewData["state"] = "لیگ نقره";
        }
        else
        {
            ViewData["state"] = "لیگ طلا";

        }

        ViewData["Score"] = CurrentUser.Score;
        if (CurrentUser.ImageName != "null")
        {
            ViewData["Image"] = CurrentUser.ImageName;
        }
        else
        {
            ViewData["Image"] = "profile.png";
        }

        var Notifications = _dbcontext.Notifications.ToList();
        ViewBag.Class0 = true;
        return View(Notifications);
    }

    [Route("/Logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [Route("/Account/Setting")]
    public IActionResult Setting()
    {
        var UserId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        var competitions = _dbcontext.Competitions.ToList();
        var CurrentUser = _dbcontext.Users.SingleOrDefault(u => u.Id == UserId);
        if (CurrentUser.ImageName != "null")
        {
            ViewData["Image"] = CurrentUser.ImageName;
        }
        else
        {
            ViewData["Image"] = "profile.png";
        }

        ViewBag.Class1 = true;
        ViewData["competitions"] = competitions;
        return View(CurrentUser);
    }

    [Route("/test")]
    [Produces("application/json")]
    public IActionResult Test()
    {
        return Json(true);
    }

    [HttpPost("/Setting")]
    public IActionResult Setting(string password)
    {
        var UserId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        var CurrentUser = _dbcontext.Users.SingleOrDefault(u => u.Id == UserId);
        CurrentUser.Password = _passswordHash.HashPassword(password);
        _dbcontext.Users.Update(CurrentUser);
        _dbcontext.SaveChanges();
        TempData["message"] = "رمز عبور تغییر کرد";
        return RedirectToAction("Setting", "Account");
    }

    [HttpPost("/Add")]
    public IActionResult Add(int op)
    {
        var UserId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        var competition = _dbcontext.Competitions.SingleOrDefault(i => i.Id == op);
        var CurrentUser = _dbcontext.Users.SingleOrDefault(u => u.Id == UserId);
        CurrentUser.Competition = new List<Competition>() { competition };
        _dbcontext.Users.Update(CurrentUser);
        try
        {
            _dbcontext.SaveChanges();
            return RedirectToAction("Setting", "Account");
        }
        catch (Exception e)
        {
            TempData["messageAdmin"] = "دراین مسابقه قبلا ثبت نام کرده اید";
            return RedirectToAction("Setting", "Account");
        }

    }

    [HttpPost("/Upload")]
    public IActionResult Upload(IFormFile formFile)
    {
        var UserId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        var CurrentUser = _dbcontext.Users.SingleOrDefault(u => u.Id == UserId);
        string FileName = Guid.NewGuid().ToString("n") + "-" + formFile.FileName;
        string TargetPath = Path.Combine(_webHostEnvironment.WebRootPath, "Admin", "assets", "img", "user", FileName);
        using (var stream = new FileStream(TargetPath, FileMode.Create))
        {
            formFile.CopyTo(stream);
        }

        CurrentUser.ImageName = FileName;
        _dbcontext.Users.Update(CurrentUser);
        _dbcontext.SaveChanges();
        TempData["messageAdmin"] = "عکس پروفایل تغییر کرد";
        return RedirectToAction("Setting", "Account");
    }

    public async Task<IActionResult> Delete()
    {
        var UserId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        var CurrentUser = _dbcontext.Users.SingleOrDefault(u => u.Id == UserId);
        _dbcontext.Users.Remove(CurrentUser);
        _dbcontext.SaveChanges();
        await HttpContext.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [Route("/Account/Competitions")]
    public IActionResult Competitions()
    {
        var UserId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        var CurrentUser = _dbcontext.Users.SingleOrDefault(u => u.Id == UserId);
        if (CurrentUser.ImageName != "null")
        {
            ViewData["Image"] = CurrentUser.ImageName;
        }
        else
        {
            ViewData["Image"] = "profile.png";
        }

        var Competitions = _dbcontext.Users.Where(u=>u.Id==CurrentUser.Id).SelectMany(u=>u.Competition).ToList();
        
        ViewData["CurrentUser"] = CurrentUser;
        ViewBag.Class2 = true;
        return View(Competitions);
    }

    [Route("/Account/Rank")]
    public IActionResult Rank()
    {
        var UserId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        var CurrentUser = _dbcontext.Users.SingleOrDefault(u => u.Id == UserId);
        if (CurrentUser.ImageName != "null")
        {
            ViewData["Image"] = CurrentUser.ImageName;
        }
        else
        {
            ViewData["Image"] = "profile.png";
        }

        var users = _dbcontext.Users.OrderByDescending(u => u.Score).ToList();
        ViewBag.Class3 = true;
        return View(users);
    }

    [Route("/Account/League")]
    public IActionResult League()
    {
        var UserId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        var CurrentUser = _dbcontext.Users.SingleOrDefault(u => u.Id == UserId);
        var score = CurrentUser.Score;
        int league = 0;
        if (CurrentUser.ImageName != "null")
        {
            ViewData["Image"] = CurrentUser.ImageName;
        }
        else
        {
            ViewData["Image"] = "profile.png";
        }

        if (score >= 0 && score <= 500)
        {
            league = 3;
        }
        else if (score > 500 && score <= 1000)
        {
            league = 2;
        }
        else
        {
            league = 1;
        }

        ViewBag.Class4 = true;
        return View(league);
    }

    [Route("/Account/Competition/{id?}")]
    public IActionResult Competition(int id)
    {
        var UserId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        var CurrentUser = _dbcontext.Users.SingleOrDefault(u => u.Id == UserId);
        if (CurrentUser.ImageName != "null")
        {
            ViewData["Image"] = CurrentUser.ImageName;
        }
        else
        {
            ViewData["Image"] = "profile.png";
        }

        var Questions = _dbcontext.Competitions.Where(i => i.Id == id).SelectMany(i => i.Question).ToList();
        ViewData["Questions"] = Questions;
        return View();
    }

    [HttpPost("/check")]
    public IActionResult check(AnswerViewModel Answer)
    {
        if (ModelState.IsValid)
        {
            var UserId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var CurrentUser = _dbcontext.Users.SingleOrDefault(u => u.Id == UserId);
            var Question = _dbcontext.Questions.SingleOrDefault(i => i.Id == Answer.QuestionId);
            if (Answer.Answer1 == Question.AnswerOne && Answer.Answer2 == Question.AnswerTwo)
            {
                CurrentUser.Score = CurrentUser.Score + 50;
                CurrentUser.IsAdmin = true;
                _dbcontext.Users.Update(CurrentUser);
                _dbcontext.SaveChanges();
                TempData["messageAdmin"] = "مسابقه با موفقیت به پایان رسید";
                return RedirectToAction("Competitions", "Account");

            }
            else
            {
                CurrentUser.IsAdmin = true;
                _dbcontext.Users.Update(CurrentUser);
                _dbcontext.SaveChanges();
                TempData["messageAdmin"] = "مسابقه بدون امتیاز به پایان رسید";
                return RedirectToAction("Competitions", "Account");
            }
        }
        else
        {
            TempData["messageAdmin"] = "جواب های شما غیر مجاز است";
            return RedirectToAction("Competitions", "Account");
        }
    }

    [Route("/Account/Helper")]
    public IActionResult Helper()
    {
        var UserId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        var CurrentUser = _dbcontext.Users.SingleOrDefault(u => u.Id == UserId);
        if (CurrentUser.ImageName != "null")
        {
            ViewData["Image"] = CurrentUser.ImageName;
        }
        else
        {
            ViewData["Image"] = "profile.png";
        }
        ViewBag.Class = true;
        return View();
    }
    public IActionResult Tasks()
    {
        var UserId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        var CurrentUser = _dbcontext.Users.SingleOrDefault(u => u.Id == UserId);
        if (CurrentUser.ImageName != "null")
        {
            ViewData["Image"] = CurrentUser.ImageName;
        }
        else
        {
            ViewData["Image"] = "profile.png";
        }
        ViewBag.Class5 = true;
        return View();
    }
}