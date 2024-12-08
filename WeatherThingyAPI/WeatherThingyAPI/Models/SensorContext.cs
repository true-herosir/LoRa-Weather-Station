using Microsoft.EntityFrameworkCore;

namespace WeatherThingyAPI.Models
{
    public class SensorContext : DbContext
    {
        public SensorContext(DbContextOptions<SensorContext> options)
            : base(options)
        {
        }

        // DbSet representing the Gateway_location table
        public DbSet<Sensor_location> Sensor_locations { get; set; } = null!;

        // Configure model using Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Specify schema and primary key
            modelBuilder.Entity<Sensor_location>()
                .ToTable("Sensor_location", schema: "lr2")  // Table in "lr2" schema
                .HasKey(g => g.Node_ID);  // Set primary key

        }
    }
}
