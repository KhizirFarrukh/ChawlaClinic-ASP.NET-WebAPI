using ChawlaClinic.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;

namespace ChawlaClinic.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
                .HasOne(d => d.PatientDiscount)
                .WithMany(p => p.Patients)
                .HasForeignKey(d => d.DiscountId);
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Patient> Patients => Set<Patient>();
        public DbSet<PatientDiscount> PatientDiscounts => Set<PatientDiscount>();
    }
}
