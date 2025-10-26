using Microsoft.EntityFrameworkCore;
using TareaReposicionSecure.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Xml.Linq;
using TareaReposicionSecure.Models;

namespace TareaReposicionSecure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users => Set<User>();
        public DbSet<Hospital> Hospitals => Set<Hospital>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>();
            modelBuilder.Entity<Hospital>();
        }
    }
}
