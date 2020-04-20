using Microsoft.EntityFrameworkCore;
using SwaggerAspCoreOData.DBContext;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace UnitTests.Mocks
{
  [ExcludeFromCodeCoverage]
  public class SampleDbContextMock
  {
    public SampleDbContext GetDbContext()
    {
      var options = new DbContextOptionsBuilder<SampleDbContext>()
               .UseInMemoryDatabase(databaseName: "SampleDb")
               .EnableDetailedErrors()
               .EnableSensitiveDataLogging()
               .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
               .Options;

      var mockDb = new SampleDbContext(options);
      mockDb.Database.EnsureCreated();
    
      return mockDb;
    }

  }
}
