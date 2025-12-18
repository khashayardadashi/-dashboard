using System.ComponentModel.DataAnnotations;

namespace persian_code.web.ViewModels;

public class LoginViewModel
{

    [EmailAddress(ErrorMessage ="آدرس ایمیل معتبر نیست")]
    [Required(ErrorMessage = "آدرس ایمیل الزامی است")]
    [MaxLength(50,ErrorMessage = "باید حداکثر 50 کارکتر باشد")]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "رمز عبور الزامی است")]
    [MaxLength(50,ErrorMessage = "باید حداکثر 50 کارکتر باشد")]
    public string Password { get; set; }
    public bool Remember { get; set; }
}