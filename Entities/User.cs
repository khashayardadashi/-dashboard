using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;
using persian_code.web.dbcontext;

namespace persian_code.web.Entities;

public class User
{
    [Key]
    public int Id { get; set; }
    [MaxLength(255)]
    public string Username { get; set; }
    [MaxLength(255)]
    public string Email { get; set; }
    [MaxLength(255)]
    public string Password { get; set; }
    public int Score { get; set; }
    [MaxLength(500)]
    public string ActiveCode { get; set; }
    public bool IsActive { get; set; }
    public DateTime Jointime { get; set; }
    public bool IsAdmin { get; set; }
    public string ImageName { get; set; }
    public ICollection<Competition> Competition { get; set; }
    
    
    
}