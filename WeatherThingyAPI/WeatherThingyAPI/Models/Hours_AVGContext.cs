using Microsoft.EntityFrameworkCore;

namespace WeatherThingyAPI.Models
{
    public class Hours_AVGContext : DbContext
    {
        public Hours_AVGContext(DbContextOptions<Hours_AVGContext> options)
           : base(options)
        {
        }

        public DbSet<Hours_AVG> Hours_AVGs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hours_AVG>().ToTable("hours_avg", schema: "lr2").HasKey(n => new { n.Node_ID, n.the_day, n.the_hour });
        }
    }
}
