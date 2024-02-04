using gRPC_Time_Server.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace gRPC_Time_Server.Data
{
    public class DataContext : DbContext
    {
        // Parameterless constructor for design-time operations
        public DataContext() : base()
        {
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<TimeRegistry> TimeRegistries { get; set; }
    }
}
