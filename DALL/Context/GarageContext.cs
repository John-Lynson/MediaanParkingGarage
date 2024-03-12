using CORE.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALL.Context
{
    public class GarageContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Garage> Garages { get; set; }
        public DbSet<ParkingSpot> ParkingSpots { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<SpotOccupation> SpotOccupations { get; set; }
        public DbSet<Tariff> Tariffs { get; set; }

        public GarageContext(DbContextOptions options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().ToTable("account");
            modelBuilder.Entity<Car>().ToTable("car");
            modelBuilder.Entity<SpotOccupation>().ToTable("spot_occupation");
            modelBuilder.Entity<Payment>().ToTable("payment_history");
            modelBuilder.Entity<ParkingSpot>().ToTable("parking_spot");
            modelBuilder.Entity<Garage>().ToTable("parking_garage");
            modelBuilder.Entity<Tariff>().ToTable("tariff");
        }
    }
}
