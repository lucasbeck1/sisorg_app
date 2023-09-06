using Microsoft.EntityFrameworkCore;

namespace api.Models

{
    public class SisorgContext : DbContext
    {
        public SisorgContext(DbContextOptions<SisorgContext> options)
            : base(options)
        {
        }
        public DbSet<Marker> Markers { get; set; } = null!;
        public DbSet<Country> Countries { get; set; } = null!;
    }
}
