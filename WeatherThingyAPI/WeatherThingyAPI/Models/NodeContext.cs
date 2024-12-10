using Microsoft.EntityFrameworkCore;

namespace WeatherThingyAPI.Models
{
    public class NodeContext : DbContext
    {
        public NodeContext(DbContextOptions<NodeContext> options)
            : base(options)
        {
        }

        public DbSet<Node> Nodes { get; set; } = null!;
        public DbSet<Node_location> SensorLocations { get; set; }  // Add SensorLocations here

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Node>().ToTable("Node", schema: "lr2").HasKey(n => new { n.Node_ID, n.Time });
            modelBuilder.Entity<Node_location>()
               .ToTable("Node_location", schema: "lr2")  // Table in "lr2" schema
               .HasKey(g => g.Node_ID);  // Set primary key
        }
    }
}
