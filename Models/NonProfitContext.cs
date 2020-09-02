using Microsoft.EntityFrameworkCore;

namespace NonProfit.Models
{
    public class NonProfitContext : DbContext
    {
        public NonProfitContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Time> Times { get; set; }

        
    }
}
