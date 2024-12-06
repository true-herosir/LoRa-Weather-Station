using Microsoft.EntityFrameworkCore;

namespace WeatherThingyAPI.Models
{
    public class Max_MinContext : DbContext
    {
        public Max_MinContext(DbContextOptions<Max_MinContext> options)
            : base(options)
        {
        }

        public DbSet<Max_Min> Max_Mins { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Max_Min>().ToTable("max_min", schema: "lr2").HasKey(n => new { n.Node_ID, n.the_day });
        }
    }
}
