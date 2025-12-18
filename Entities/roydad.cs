using System.ComponentModel.DataAnnotations;

namespace persian_code.web.Entities;

public class roydad
{
    [Key]
    public int Id { get; set; }
    [MaxLength(255)]
    public string Title { get; set; }
    
    [MaxLength(500)]
    public string Discription { get; set; }
    public string ImageName { get; set; }
    
    
}