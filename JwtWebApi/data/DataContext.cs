using JwtWebApi.tables;
using Microsoft.EntityFrameworkCore;

namespace JwtWebApi.data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Hero> AllHeroes { get; set; }



    }
}
