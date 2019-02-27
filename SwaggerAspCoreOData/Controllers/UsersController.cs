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

namespace SwaggerAspCoreOData.Controllers
{
  [ApiVersion("1.0")]
  [ApiVersion("3.0")]
  public class UsersController : ODataController
  {
    [HttpGet]
    [EnableQuery]
    public IQueryable<User> Get()
    {
      return new List<User>
      {
        new User
        {
           Id = 10,
           Username = "Kuda",
           Profile = "Admin"
        }
      }.AsQueryable();
    }
  }

  
}