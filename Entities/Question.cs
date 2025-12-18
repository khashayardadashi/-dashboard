using System.ComponentModel.DataAnnotations;

namespace persian_code.web.Entities;

public class Question
{
    [Key]
    public int Id { get; set; }
    [MaxLength(255)]
    public string Title { get; set; }
    [MaxLength(1000)]
    public string Description { get; set; }
    [MaxLength(255)]
    public string InputOne { get; set; }
    [MaxLength(255)]
    public string InputTwo { get; set; }
    [MaxLength(255)]
    public string AnswerOne { get; set; }
    [MaxLength(255)]
    public string AnswerTwo { get; set; }

    #region Navigation-part

    public Competition Competition { get; set; }

    #endregion
   
}