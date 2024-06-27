using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI;
using TravelAgencyApi.Models;

namespace TravelAgencyApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("Server=db4free.net;Database=travelagencydb;User=grrr_human;Password=1234567890;SSL Mode=None");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tourist>()
                .HasOne(t => t.TouristData)
                .WithOne(td => td.Tourist)
                .HasForeignKey<TouristData>(td => td.IdTourist)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.EmployeeData)
                .WithOne(ed => ed.Employee)
                .HasForeignKey<EmployeeData>(ed => ed.IdEmployee)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Country>()
                .HasMany(c => c.Cities)
                .WithOne(e => e.Country)
                .HasForeignKey(e => e.IdCountry)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<City>()
                .HasMany(c => c.Residences)
                .WithOne(r => r.City)
                .HasForeignKey(r => r.IdCity)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TypeMeal>()
                .HasMany(tm => tm.Tours)
                .WithOne(t => t.TypeMeal)
                .HasForeignKey(t => t.IdTypeMeal)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TouristTour>()
                .HasOne(tt => tt.Tourist)
                .WithMany(t => t.TouristsTours)
                .HasForeignKey(tt => tt.IdTourist);

            modelBuilder.Entity<TouristTour>()
                .HasOne(tt => tt.Tour)
                .WithMany(t => t.TouristsTours)
                .HasForeignKey(tt => tt.IdTour);

            modelBuilder.Entity<Residence>()
                .HasMany(r => r.Tours)
                .WithOne(t => t.Residence)
                .HasForeignKey(t => t.IdResidence)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TourOperator>()
                .HasMany(to => to.Tours)
                .WithOne(t => t.TourOperator)
                .HasForeignKey(t => t.IdTourOperator)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Tour>()
                .HasMany(t => t.Bookings)
                .WithOne(b => b.Tour)
                .HasForeignKey(b => b.IdTour)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Bookings)
                .WithOne(b => b.Employee)
                .HasForeignKey(b => b.IdEmployee)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Tourist>()
                .HasMany(c => c.Bookings)
                .WithOne(b => b.Tourist)
                .HasForeignKey(b => b.IdTourist)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Booking> Bookings { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeData> EmployeeData { get; set; }
        public DbSet<Residence> Residences { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<Tourist> Tourists { get; set; }
        public DbSet<TouristData> TouristData { get; set;}
        public DbSet<TouristTour> TouristsTours { get; set; }
        public DbSet<TourOperator> TourOperators { get; set; }
        public DbSet<TypeMeal> TypesMeal { get; set; }

    }
}
