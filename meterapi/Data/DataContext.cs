using meterapi.Models;
using Microsoft.EntityFrameworkCore;

namespace meterapi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<UserMeter> UserMeters { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Meter> Meters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Meter>()
       .HasAlternateKey(m => new { m.MeterId, m.RpId });


            modelBuilder.Entity<UserMeter>()
        .HasOne(um => um.Meter)
        .WithMany(m => m.UserMeters)
        .HasForeignKey(um => um.MeterId)
        .HasForeignKey(um => um.UserId);

          
        }
    }
}
