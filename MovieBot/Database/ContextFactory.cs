using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Configuration;

namespace MovieBot.Database
{
    public class ContextFactory : IDesignTimeDbContextFactory<UserDatabaseContext>
    {
        public UserDatabaseContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<UserDatabaseContext> optionsBuilder = new DbContextOptionsBuilder<UserDatabaseContext>();
            optionsBuilder.UseMySql(ConfigurationManager.ConnectionStrings["UsersMySqlDatabaseConnection"].ConnectionString, new MySqlServerVersion(new Version()));
            return new UserDatabaseContext(optionsBuilder.Options);
        }
    }
}
