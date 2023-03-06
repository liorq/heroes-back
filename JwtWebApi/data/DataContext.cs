using JwtWebApi.tables;
using Microsoft.EntityFrameworkCore;

namespace JwtWebApi.data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Guests> Guests { get; set; }
        public DbSet<Messages> Messages { get; set; }
        public DbSet<Missions> Missions { get; set; }
        public DbSet<Rooms> Rooms { get; set; }
        public DbSet<Hero> UsersHeroes { get; set; }
        public DbSet<Hero> AllHeroes { get; set; }

    }
}
