using Microsoft.AspNetCore.Mvc;
using persian_code.web.dbcontext;

namespace persian_code.web.Controllers;

public class ActiveEmailController : Controller
{
    private readonly Applicationdbcontext _dbcontext;
    public ActiveEmailController(Applicationdbcontext dbcontext)
    {
        _dbcontext = dbcontext;
    }
    [HttpGet("/ActiveEmail/{ActiveCode?}")]
    public IActionResult Index(string ActiveCode)
    {
        var UserActive = _dbcontext.Users.SingleOrDefault(u => u.ActiveCode == ActiveCode);
        if (UserActive != null)
        {
            UserActive.ActiveCode = Guid.NewGuid().ToString("N");
            UserActive.IsActive = true;
            _dbcontext.Users.Update(UserActive);
            _dbcontext.SaveChanges();
            TempData["message"] = "ایمیل با موفقیت تایید شد";
            return RedirectToAction("Index", "Login");
        }
        else
        {
            return NotFound();
        }
    }
}