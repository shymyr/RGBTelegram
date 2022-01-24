using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RGBTelegram.Entities;

namespace RGBTelegram
{
    public class DataContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public DataContext()
        {
        }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { Database.EnsureCreated(); }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlServer("Data Source=DESKTOP-NE0KK05;Initial Catalog=vpluse;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        public DbSet<AppUser> Users { get; set; }

        public DbSet<UserSession> UserSessions { get; set; }

        public DbSet<AuthData> AuthDatas { get; set; }
        public DbSet<Registration> Registrations { get; set; }
    }
}
