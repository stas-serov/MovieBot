using Microsoft.EntityFrameworkCore;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace MovieBot.Database
{
    public class MySQLDatabase
    {
        UserDatabaseContext context { get; set; }

        public MySQLDatabase() 
        { 
            context = new UserDatabaseContext(GetContextOptions());
        }

        public async Task<User?> GetUser(long id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<bool> DeleteUser(long id)
        {
            User? userToDelete = await context.Users.FindAsync(id);
            if(userToDelete != null)
            {
                context.Users.Remove(userToDelete);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async void AddUser(User user)
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

        public async void UpdateUser(User user)
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }

        private DbContextOptions<UserDatabaseContext> GetContextOptions()
        {
            DbContextOptionsBuilder<UserDatabaseContext> optionsBuilder = new DbContextOptionsBuilder<UserDatabaseContext>();
            DbContextOptions<UserDatabaseContext> contextOptions = optionsBuilder.UseMySql(ConfigurationManager.ConnectionStrings["UsersMySqlDatabaseConnection"].ConnectionString , new MySqlServerVersion(new Version())).Options;
            return contextOptions;
        }
    }
}
