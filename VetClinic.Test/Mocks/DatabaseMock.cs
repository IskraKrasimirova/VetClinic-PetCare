using Microsoft.EntityFrameworkCore;
using System;
using VetClinic.Data;

namespace VetClinic.Test.Mocks
{
    public class DatabaseMock
    {
        public static VetClinicDbContext Instance
        {
            get
            {
                var options = new DbContextOptionsBuilder<VetClinicDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                    .Options;
                var dbContext = new VetClinicDbContext(options);

                return dbContext;
            }
        }
    }
}
