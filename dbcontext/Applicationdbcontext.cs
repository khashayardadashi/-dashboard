using Microsoft.EntityFrameworkCore;
using persian_code.web.Entities;

namespace persian_code.web.dbcontext
{
	public class Applicationdbcontext :DbContext
	{
		public Applicationdbcontext(DbContextOptions<Applicationdbcontext> options):base(options){}
		public DbSet<User> Users { get; set; }
		public DbSet<Competition> Competitions { get; set; }
		public DbSet<Question> Questions { get; set; }
		public DbSet<Notification> Notifications { get; set; }
		public DbSet<roydad> Roydad { get; set; }
    }
}
