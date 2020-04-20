using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SwaggerAspCoreOData.Controllers;
using SwaggerAspCoreOData.Models;
using SwaggerAspCoreOData.Repositories;
using System;
using System.Linq;
using UnitTests.Mocks;
using Xunit;

namespace UnitTests
{
  public class UsersControllerTests
  {    
    private readonly SampleDbEntitiesMock sampleDbEntities;   
    private readonly Mock<IUserRepository> userRepository;

    public UsersControllerTests()
    {     
      sampleDbEntities = new SampleDbEntitiesMock();      
      userRepository = new Mock<IUserRepository>();      
    }

    [Fact]
    public void TestGetAllUsers()
    {
      // Arrange   
      userRepository.Setup(x => x.GetUserRoles()).Returns(sampleDbEntities.GetTestUsers());
      var controller = new UsersController(userRepository.Object);
     
      // Act
      var result = controller.Get();

      // Assert
      Assert.NotNull(result);
      Assert.Equal(result.Count(), sampleDbEntities.GetTestUsers().Count); 
    }

    [Fact]
    public void TestGetUserByKey()
    {
      int key = 1;

      // Arrange    
      userRepository.Setup(x => x.GetByKey(key)).Returns(sampleDbEntities.GetTestSingleUser());
      var controller = new UsersController(userRepository.Object);

      // Act
      var result = controller.Get(key);

      // Assert
      Assert.NotNull(result);
      Assert.Equal(result.Queryable.FirstOrDefault().Username, sampleDbEntities.GetTestSingleUser().Name);
      Assert.IsType<SingleResult<User>>(result);
    }

    [Fact]
    public void TestCreateUser()
    {

      // Arrange
      var user = new User
      {
        Id = 10,
        Email = "jimjones@you.com",
        Username = "Jim",
        Profile = "Everyone"
      };
    
      userRepository.Setup(x => x.Add(It.IsAny<SwaggerAspCoreOData.DBContext.Users>()));
      var controller = new UsersController(userRepository.Object);

      // Act
      var result = controller.Post(user);

      // Assert
      Assert.NotNull(result);
      Assert.IsType<CreatedODataResult<SwaggerAspCoreOData.DBContext.Users>>(result);
    }

    [Fact]
    public void TestPatchUser()
    {

      // Arrange     
      int key = 1;
      var delta = new Delta<User>(typeof(User));  
      
      userRepository.Setup(x => x.Update(It.IsAny<SwaggerAspCoreOData.DBContext.Users>()));
      userRepository.Setup(x => x.GetByKey(key)).Returns(sampleDbEntities.GetTestSingleUser());

      var controller = new UsersController(userRepository.Object);

      // Act
      var result = controller.Patch(key, delta);

      // Assert
      Assert.NotNull(result);
      Assert.IsType<OkResult>(result);
    }

    [Fact]
    public void TestDeleteUser()
    {

      // Arrange     
      int key = 1;
      var delta = new Delta<User>(typeof(User));

      userRepository.Setup(x => x.Delete(It.IsAny<SwaggerAspCoreOData.DBContext.Users>()));
      userRepository.Setup(x => x.GetByKey(key)).Returns(sampleDbEntities.GetTestSingleUser());

      var controller = new UsersController(userRepository.Object);

      // Act
      var result = controller.Delete(key);

      // Assert
      Assert.NotNull(result);
      Assert.IsType<OkResult>(result);
    }
  }
}
