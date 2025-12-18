using System.Security.Cryptography;
using cvweb.Services;
using cvweb.Services.Intefaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using persian_code.web.dbcontext;
using persian_code.web.Entities;
using persian_code.web.ViewModels;

namespace persian_code.web.Controllers
{
    public class RegisterController : Controller
    {
	    private readonly Applicationdbcontext _dbcontext;
	    private readonly IPassswordHash _passwordHash;
	    private readonly IEmailsender _emailsender;
	    public RegisterController(Applicationdbcontext dbcontext , IEmailsender emailsender , IPassswordHash passwordHash)
	    {
		    _dbcontext = dbcontext;
		    _emailsender = emailsender;
		    _passwordHash = passwordHash;
	    }
        [HttpGet("/Register")]
        public IActionResult Index()
        {
            return View();
        }
		[HttpPost("/Register")]
		public IActionResult Index(RegisterViewModel Register)
		{
			if (ModelState.IsValid)
			{ 
				var user_by_email = _dbcontext.Users.SingleOrDefault(u => u.Email == Register.Email);
				var user_by_name = _dbcontext.Users.SingleOrDefault(u => u.Username == Register.Username);
				
				if(user_by_name !=null && user_by_email!=null)
				{
					TempData["message"]="شما ثبت نام کرده اید لطفا وارد شوید";
					return RedirectToAction("Index", "Register");
				}
				else if (user_by_email != null)
				{
					ModelState.AddModelError("Email","آدرس ایمیل تکراری است");
					return View();
				}
				else if(user_by_name !=null)
				{
					ModelState.AddModelError("Username" ,"نام کاربری تکراری است" );
					return View();
				}
				else
				{
					var newUser = new User
					{

						Username = Register.Username,
						Email = Register.Email,
						Password = _passwordHash.HashPassword(Register.Password),
						ActiveCode = Guid.NewGuid().ToString("N"),
						IsActive = false,
						Score = 0,
						Jointime = DateTime.Today.Date,
						IsAdmin = false,
						ImageName = "null",
					};	
					_dbcontext.Users.Add(newUser);
					_dbcontext.SaveChanges();
					_emailsender.SendMail(Register.Email, "تایید آدرس ایمیل",
						$"<p dir=\"rtl\"> کاربر {Register.Username} عزیز </p>" +
                        "<p dir=\"rtl\">با تایید آدرس ایمیل , حساب کاربری خود را فعال کنید</p>" +
						$"<p dir=\"rtl\"><a  href='http://localhost:5197/ActiveEmail/{newUser.ActiveCode}'>لینک تایید ایمیل</a></p>"
                  
                        );
					TempData["message"] = "جهت تایید حساب ایمیل خود را چک کنید";
					return RedirectToAction("Index", "Login");
				}
			}
			else
			{
				TempData["message"] = "ثبت نام با خطا روبرو شد";
				return View();
			}
		}
	}
}
