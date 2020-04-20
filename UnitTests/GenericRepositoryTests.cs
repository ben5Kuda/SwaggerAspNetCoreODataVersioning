using Moq;
using SwaggerAspCoreOData.Infastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnitTests.Mocks;
using Xunit;

namespace UnitTests
{
  public class GenericRepositoryTests
  {
    private readonly SwaggerAspCoreOData.DBContext.SampleDbContext sampleDbContext;
    private readonly SampleDbContextMock sampleDbContextMock;
    private readonly Mock<IRepository<SwaggerAspCoreOData.DBContext.Users>> repository;

    public GenericRepositoryTests()
    {
      sampleDbContextMock = new SampleDbContextMock();
      sampleDbContext = sampleDbContextMock.GetDbContext();
      repository = new Mock<IRepository<SwaggerAspCoreOData.DBContext.Users>>();
    }

    [Fact]
    public void TestGetAll()
    {
      
      var mockContext = sampleDbContext;

      var user = new SwaggerAspCoreOData.DBContext.Users
      {
        Id = 12,
        Email = "wes@you.com",
        Name = "Wes",
        Profile = "Everyone",
      };
    
      // Add test entity to in memory database and save the changes
      mockContext.Add(user);
      mockContext.SaveChanges();  
      
      var repo = new Repository<SwaggerAspCoreOData.DBContext.Users>(sampleDbContext);
      
      // Act
      var result = repo.GetAll();

      // Assert
      Assert.NotNull(result);
      Assert.True(result.Count() >= 1);
    }

    [Fact]
    public void TestGetByKey()
    {

      var mockContext = sampleDbContext;
      var user = new SwaggerAspCoreOData.DBContext.Users
      {
        Id = 5,
        Email = "kuda@you.com",
        Name = "Kuda",
        Profile = "Everyone",
      };

      // Add test entity to in memory database and save the changes
      mockContext.Add(user);
      mockContext.SaveChanges();

      var repo = new Repository<SwaggerAspCoreOData.DBContext.Users>(sampleDbContext);

      int key = user.Id;

      // Act
      var result = repo.GetByKey(key);

      // Assert
      Assert.NotNull(result);
      var dbUser = mockContext.Users.FirstOrDefault(x => x.Id == key);

      //compare results to in memory db
      Assert.Equal(result.Name, dbUser.Name);
    }

    [Fact]
    public void TestAdd()
    {

      var mockContext = sampleDbContext;

      var user = new SwaggerAspCoreOData.DBContext.Users
      {
        Id = 6,
        Email = "mike@you.com",
        Name = "Mike",
        Profile = "Everyone",
      };

      // get number of entities before add
      var preAdditionCount = mockContext.Users.Count();

      var repo = new Repository<SwaggerAspCoreOData.DBContext.Users>(sampleDbContext);
    
      // Act
      repo.Add(user);
      repo.Save();

      var postAdditionCount = mockContext.Users.Count();

     // confirm if a user has been added
      Assert.True(postAdditionCount == (preAdditionCount + 1));
    }


    [Fact]
    public void TestUpdate()
    {

      var mockContext = sampleDbContext;

      var user = new SwaggerAspCoreOData.DBContext.Users
      {
        Id = 9,
        Email = "jen@you.com",
        Name = "Jen",
        Profile = "Everyone",
      };

      var key = user.Id;

      // Add test entity to in memory database and save the changes
      mockContext.Add(user);
      mockContext.SaveChanges();

      // confirm the current email before update in memory database
      Assert.True(mockContext.Users.Find(key).Email == "jen@you.com");

      var repo = new Repository<SwaggerAspCoreOData.DBContext.Users>(sampleDbContext);

      // modify email
      user.Email = "jen@netcore.com";

      // Act
      repo.Update(user);
      repo.Save();

      var updatedUser = repo.GetByKey(key);

      // confirm if a user email has been modified
      Assert.True(updatedUser.Email == "jen@netcore.com");
    }

    [Fact]
    public void TestDelete()
    {

      var mockContext = sampleDbContext;

      var user = new SwaggerAspCoreOData.DBContext.Users
      {
        Id = 10,
        Email = "helga@you.com",
        Name = "Helga",
        Profile = "Everyone",
      };

      // Add test entity to in memory database and save the changes
      mockContext.Add(user);
      mockContext.SaveChanges();

      // confirm user exists
      var preDeletionCount = mockContext.Users.Count();

      var repo = new Repository<SwaggerAspCoreOData.DBContext.Users>(sampleDbContext);
          
      // Act
      repo.Delete(user);
      repo.Save();

      var postDeletionCount = mockContext.Users.Count();

      // confirm if user is deleted.
      Assert.NotEqual(postDeletionCount, preDeletionCount);
      Assert.True((preDeletionCount - 1) == postDeletionCount);
    }


  }
}
