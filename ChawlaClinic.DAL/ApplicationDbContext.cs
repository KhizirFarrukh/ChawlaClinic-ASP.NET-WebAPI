using ChawlaClinic.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChawlaClinic.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
    }
}
