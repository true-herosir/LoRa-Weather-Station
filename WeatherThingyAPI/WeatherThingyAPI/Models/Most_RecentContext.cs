using Microsoft.EntityFrameworkCore;

namespace WeatherThingyAPI.Models
{
    public class Most_RecentContext : DbContext
    {
        public Most_RecentContext(DbContextOptions<Most_RecentContext> options)
            : base(options)
        {
        }

        public DbSet<Most_Recent> Most_Recents { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Most_Recent>().ToTable("most_recent", schema: "lr2").HasKey(n => new { n.Node_ID});
        }
    }
}
