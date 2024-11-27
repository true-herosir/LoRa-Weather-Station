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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Node>().ToTable("Node", schema: "lr2").HasKey(n => new { n.Node_ID, n.Time });
        }
    }
}
