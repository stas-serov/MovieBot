using Microsoft.EntityFrameworkCore;

namespace MovieBot.Database
{
    public class UserDatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserDatabaseContext(DbContextOptions<UserDatabaseContext> options) : base(options){ }
    }
}
