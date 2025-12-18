using System.ComponentModel.DataAnnotations;

namespace persian_code.web.ViewModels;

public class AnswerViewModel
{
    [MaxLength(20 , ErrorMessage = "حداکثر 20 کارکتر باشد")]
    [Required(ErrorMessage = "وارد کردن جواب الزامی است")]
    public string Answer1 { get; set; }
    [MaxLength(20 , ErrorMessage = "حداکثر 20 کارکتر باشد")]
    [Required(ErrorMessage = "وارد کردن جواب الزامی است")]
    public string Answer2 { get; set; }
    public int QuestionId { get; set; }
}