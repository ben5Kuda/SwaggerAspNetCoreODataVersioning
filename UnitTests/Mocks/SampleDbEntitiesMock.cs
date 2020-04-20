
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.Mocks
{
  public class SampleDbEntitiesMock
  {
    public ICollection<SwaggerAspCoreOData.DBContext.Users> GetTestUsers()
    {
      return new List<SwaggerAspCoreOData.DBContext.Users>
      {
        new SwaggerAspCoreOData.DBContext.Users
        {
          Id = 1,
          Email = "kuda@you.com",
          Name = "Kuda",
          Profile = "Everyone",
          UserRoles = new List<SwaggerAspCoreOData.DBContext.UserRoles>
          {
            new SwaggerAspCoreOData.DBContext.UserRoles
            {
              RoleId = 12,
              UserId = 1
            }
          }
        }
      };
    }

    public SwaggerAspCoreOData.DBContext.Users GetTestSingleUser()
    {
        return new SwaggerAspCoreOData.DBContext.Users
        {
          Id = 2,
          Email = "jack@you.com",
          Name = "Jack",
          Profile = "Everyone"          
        
      };
    }
  }
}
