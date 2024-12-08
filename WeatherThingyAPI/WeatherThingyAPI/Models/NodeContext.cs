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
        public DbSet<Sensor_location> SensorLocations { get; set; }  // Add SensorLocations here

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Node>().ToTable("Node", schema: "lr2").HasKey(n => new { n.Node_ID, n.Time });
            modelBuilder.Entity<Sensor_location>()
               .ToTable("Sensor_location", schema: "lr2")  // Table in "lr2" schema
               .HasKey(g => g.Node_ID);  // Set primary key
        }
    }
}
