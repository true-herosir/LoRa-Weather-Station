using Microsoft.EntityFrameworkCore;

namespace WeatherThingyAPI.Models
{
    public class Node_locationContext : DbContext
    {
        public Node_locationContext(DbContextOptions<Node_locationContext> options)
            : base(options)
        {
        }

        // DbSet representing the Gateway_location table
        public DbSet<Node_location> Node_locations { get; set; } = null!;

        // Configure model using Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Specify schema and primary key
            modelBuilder.Entity<Node_location>()
                .ToTable("Node_location", schema: "lr2")  // Table in "lr2" schema
                .HasKey(g => g.Node_ID);  // Set primary key

        }
    }
}
