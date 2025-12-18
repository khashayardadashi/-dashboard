using System.ComponentModel.DataAnnotations;
using persian_code.web.dbcontext;

namespace persian_code.web.Entities;

public class Competition
{
    private readonly Applicationdbcontext _dbcontext;
    public Competition(Applicationdbcontext dbcontext)
    {
        _dbcontext = dbcontext;
    }
    [Key]
    public int Id { get; set; }
    [MaxLength(255)]
    public string Title { get; set; }
    [MaxLength(1000)]
    public string Description { get; set; }
    public string ImageName { get; set; }

    public DateTime DateTime { get; set; }
    
    public bool IsStart { get; set; }

    public bool IsFinish { get; set; }
    
    public ICollection<User> User { get; set; }
    public ICollection<Question> Question { get; set; }

    public int Countmember(int idCompetiton)
    {
        var item = _dbcontext.Competitions.Where(i => i.Id == idCompetiton).SelectMany(i => i.User).Count();
        return item;
    }
    public List<User> Showmember(int idCompetiton)
    {
        var item = _dbcontext.Competitions.Where(i => i.Id == idCompetiton).SelectMany(i => i.User).ToList();
        return item;
    }
}