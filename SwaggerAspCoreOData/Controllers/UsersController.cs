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
using SwaggerAspCoreOData.Repositories;
using Microsoft.AspNetCore.Http.Abstractions;
using Swashbuckle.AspNetCore.Annotations;

namespace SwaggerAspCoreOData.Controllers
{
  [ApiVersion("1.0")]
  [ApiVersion("3.0")]
  [Produces("application/json")]
  [SwaggerTag("Provides operations to manage users.")]
  public class UsersController : ODataController
  {
    private readonly IUserRepository _userRepository;
    public UsersController(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    [EnableQuery]
    [ODataSelectRequestHandler]   
    public IQueryable<User> Get()
    {
      return QueryAll();
    }
  
    [EnableQuery]
    public User Get(int key)
    {
      return QuerySingle(key);
    }

    public IActionResult Post([FromBody] User model)
    {
      return Created(model);
    }

    public IActionResult Delete(long key)
    {
      return Ok("Deleted");
    }

    private IQueryable<User> QueryAll()
    {
      var users = _userRepository.GetUserRoles();

      return (from u in users
              select new User
              {
                Id = u.Id,
                Username = u.Name,
                Email = u.Email,
                Profile = u.Profile,
                Roles = from r in u.UserRoles
                        select new Role
                        {
                          Id = r.RoleId
                        }

              }).AsQueryable();
    }

    private User QuerySingle(int key)
    {
      var user = _userRepository.GetByKey(key);

      return new User
      {
        Id = user.Id,
        Username = user.Name,
        Email = user.Email,
        Profile = user.Profile,
      };
    }
  }


}