using MathApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MathApp.Data
{
    public class MathAppContext : DbContext
    {
        public MathAppContext(DbContextOptions<MathAppContext> options)
            : base(options) { }

        public DbSet<MathCalculations> MathCalculations { get; set; } = default!;
        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}
