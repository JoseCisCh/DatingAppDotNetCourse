using Microsoft.EntityFrameworkCore;
using API.Entities;

namespace API.Data {

    // We have to inherit from DbContext
    public class DataContext : DbContext
    {

        // The options is a connection string to the database
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        // This represent tables inside the database. The table name in this case would be Users.
        public DbSet<AppUser> Users {get; set;}
    }
}