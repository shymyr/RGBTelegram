using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RGBTelegram.Entities;

namespace RGBTelegram
{
    public class DataContext : DbContext
    {        
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
                
        public DbSet<AppUser> Users { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<AuthData> AuthDatas { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<UZUser> UZUsers { get; set; }
        public DbSet<UZSession> UZSessions { get; set; }
        public DbSet<UZRegistration> UZRegistrations { get; set; }
        public DbSet<Token> UZAuthToken { get; set; }
    }
}
