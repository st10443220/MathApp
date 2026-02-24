using Microsoft.EntityFrameworkCore;

namespace MathApp.Data
{
    public class MathAppContext : DbContext
    {
        public MathAppContext(DbContextOptions<MathAppContext> options)
            : base(options) { }

        public DbSet<MathApp.Models.MathCalculations> MathCalculations { get; set; } = default!;
    }
}
