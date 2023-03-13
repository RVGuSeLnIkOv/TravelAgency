using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travelAgency2.Models
{
    public class MyContext : DbContext
    {
        public MyContext() : base() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseMySQL(System.Configuration.ConfigurationManager.ConnectionStrings["MyContext"].ConnectionString);
            optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=root;database=travel_agency;SSL Mode=None");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .HasOne(c => c.ClientData)
                .WithOne(cd => cd.Client)
                .HasForeignKey<ClientData>(cd => cd.IdClient)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.EmployeeData)
                .WithOne(ed => ed.Employee)
                .HasForeignKey<EmployeeData>(ed => ed.IdEmployee)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Country>()
                .HasMany(c => c.City)
                .WithOne(e => e.Country)
                .HasForeignKey(e => e.IdCountry)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<City>()
                .HasMany(c => c.Residence)
                .WithOne(r => r.City)
                .HasForeignKey(r => r.IdCity)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TypeResidence>()
                .HasMany(tr => tr.Residence)
                .WithOne(r => r.TypeResidence)
                .HasForeignKey(r => r.IdTypeResidence)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<City>()
                .HasMany(c => c.Tour)
                .WithOne(t => t.City)
                .HasForeignKey(t => t.IdCity)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Residence>()
                .HasMany(r => r.Tour)
                .WithOne(t => t.Residence)
                .HasForeignKey(t => t.IdResidence)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Tour>()
                .HasMany(t => t.Order)
                .WithOne(o => o.Tour)
                .HasForeignKey(o => o.IdTour)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employee>()
                .HasMany(e => e.Order)
                .WithOne(o => o.Employee)
                .HasForeignKey(o => o.IdEmployee)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Client>()
                .HasMany(c => c.Order)
                .WithOne(o => o.Client)
                .HasForeignKey(o => o.IdClient)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<EmployeeData> EmployeesData { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<ClientData> ClientsData { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Residence> Residences { get; set; }

        public DbSet<TypeResidence> TypesResidence { get; set; }

        public DbSet<Tour> Tours { get; set; }

        public DbSet<Order> Orders { get; set; }
    }
}
