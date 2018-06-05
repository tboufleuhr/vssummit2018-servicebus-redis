using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace VsSummit2018.Data
{
    public static class DbContextExtensions
    {
        public static bool HasChanges(this DbContext context)
        {
            return context.ChangeTracker.Entries()
                .Any(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted);
        }
    }
}
