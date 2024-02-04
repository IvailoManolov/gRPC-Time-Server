using gRPC_Time_Server.Data;
using Microsoft.EntityFrameworkCore;

namespace gRPC_Time_Server.Services
{
    public static class ServiceHelper
    {
        public static DataContext ExtractDataContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=mediumTestDB;Trusted_Connection=true;TrustServerCertificate=true;");

            return new DataContext(optionsBuilder.Options);
        }
    }
}
