using System.ComponentModel.DataAnnotations;

namespace persian_code.web.Entities;

public class Notification
{
    [Key]
    public int Id { get; set; }
    [MaxLength(255)]
    public string Title { get; set; }
    [MaxLength(1000)]
    public string Description { get; set; }
    public DateTime Date { get; set; }
}