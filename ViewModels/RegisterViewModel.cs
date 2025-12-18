using System.ComponentModel.DataAnnotations;
namespace persian_code.web.ViewModels
{
    public class RegisterViewModel
    {
        [MaxLength(50 , ErrorMessage ="نام کاربری باید حداکثر 50 کارکتر باشد")]
        [Required(ErrorMessage ="نام کاربری الزامی است")]
        public string Username { get; set; }

        [EmailAddress(ErrorMessage ="آدرس ایمیل معتبر نیست")]
        [Required(ErrorMessage = "آدرس ایمیل الزامی است")]
        [MaxLength(50,ErrorMessage = "باید حداکثر 50 کارکتر باشد")]
        public string Email { get; set; }
        
        [MaxLength(50,ErrorMessage = "باید حداکثر 50 کارکتر باشد")]
        [MinLength(8,ErrorMessage = "باید حداقل 8 کارکتر باشد")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "رمز عبور الزامی است")]
        public string Password { get; set; }

        [MaxLength(250,ErrorMessage = "باید حداکثر 50 کارکتر باشد")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="رمز عبور مطابقت ندارد")]
        [Required(ErrorMessage = "تکرار رمز عبور الزامی است")]
        public string ConfirmPassword { get; set; }
    }
}
