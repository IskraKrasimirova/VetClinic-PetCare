using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using VetClinic.Data;

namespace VetClinic.Test.ServicesTests
{
    public class InMemoryDbContext
    {
        private readonly SqliteConnection connection;
        private readonly DbContextOptions<VetClinicDbContext> dbContextOptions;

        public InMemoryDbContext()
        {
            connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            dbContextOptions = new DbContextOptionsBuilder<VetClinicDbContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new VetClinicDbContext(dbContextOptions);

            context.Database.EnsureCreated();
        }

        public VetClinicDbContext CreateContext() => new VetClinicDbContext(dbContextOptions);

        public void Dispose() => connection.Dispose();
    }
}
