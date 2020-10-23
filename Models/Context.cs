using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Models {
    public class Context : DbContext
    {
        private IConfigurationRoot Configuration { get; }

        public Context(DbContextOptions<Context> options): base(options){}

        public DbSet<User> Users { get; set; }

        public DbSet<EmailUser> EmailUsers { get; set; }
        public DbSet<Game> Games { get; set; }
    } 
}