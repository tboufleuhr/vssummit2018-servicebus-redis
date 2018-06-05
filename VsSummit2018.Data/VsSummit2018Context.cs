using VsSummit2018.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace VsSummit2018.Data
{
    public class VsSummit2018Context : DbContext
    {
        public DbSet<UserProfile> UserProfile { get; set; }

        public VsSummit2018Context(DbContextOptions<VsSummit2018Context> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public void Migrate()
        {
            Database.Migrate();
        }
    }
}
