using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwaggerAspCoreOData.Models;
using SwaggerAspCoreOData.ODataQueryOptions;

namespace SwaggerAspCoreOData.Controllers
{
  [ApiVersion("1.0")]
  [ApiVersion("3.0")]
  public class UsersController : ODataController
  {
    [HttpGet]
    [EnableQuery]
    [ODataSelectRequestHandler]
    public IQueryable<User> Get()
    {
      return Query();
    }

    [HttpGet]
    [EnableQuery]
    public SingleResult<User> Get(int key)
    {
      var model = Query().Where(x => x.Id == key);
      return SingleResult.Create(model);
    }

    private IQueryable<User> Query()
    {
      return new List<User>
      {
        new User
        {
           Id = 10,
           Username = "Kuda",
           Profile = "Admin",
           Roles = new List<Role>
           {
             new Role
             {
                Id = 1,
                RoleName = "Supervisor"
             }

           }
        }
      }.AsQueryable();
    }
  }

  
}