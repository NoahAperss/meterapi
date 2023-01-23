using meterapi.Models;
using Microsoft.EntityFrameworkCore;

namespace meterapi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

       
        public DbSet<Meter> Meters { get; set; }
        public DbSet<MeterData> MeterDatas { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserMeter> UserMeters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MeterData>()
                .HasOne(md => md.Meter)
                .WithMany(m => m.MeterDatas)
                .HasForeignKey(md => md.MeterId);

            modelBuilder.Entity<UserMeter>()
                .HasOne(um => um.Meter)
                .WithMany(m => m.UserMeter)
                .HasForeignKey(um => um.MeterId);

            modelBuilder.Entity<UserMeter>()
                .HasOne(um => um.User)
                .WithMany(u => u.UserMeters)
                .HasForeignKey(um => um.UserId);
        }
    }

}
