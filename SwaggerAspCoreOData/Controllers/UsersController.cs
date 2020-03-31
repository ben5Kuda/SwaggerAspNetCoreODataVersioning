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
using SwaggerAspCoreOData.Models.Mappers;
using SwaggerAspCoreOData.RequestValidation;

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
    public SingleResult<User> Get(int key)
    {
      var model = QuerySingle(key);
      return SingleResult.Create(model);
    }


    [ServiceFilter(typeof(ModelValidationFilterAttribute))]
    public IActionResult Post([FromBody] User model)
    {    
      var map = new UsersMapper();

      var entity = map.ToEntity(model);

      _userRepository.Add(entity);
      _userRepository.Save();

      return Created(model);
    }

    [ServiceFilter(typeof(ModelValidationFilterAttribute))]
    public IActionResult Patch(int key, [FromBody] Delta<User> delta)
    {      

      IEnumerable<string> invalidPropertyNames = delta.GetChangedPropertyNames().Except(new[]
      {
        "Email", "Profile"
      });

      if (invalidPropertyNames.Any())
      {
        foreach (string propertyName in invalidPropertyNames)
        {
          return BadRequest(propertyName + " : This field is not allowed to be updated.");
        }
      }

      var entity = _userRepository.GetByKey(key);

      if (entity == null)
      {
        return NotFound();
      }

      var map = new UsersMapper();
      var model = map.FromEntity(entity);
      delta.Patch(model);

      var userEntity = map.ToEntity(model, entity);

      _userRepository.Update(userEntity);
      _userRepository.Save();

      return Ok();

    }

    public IActionResult Delete(int key)
    {
      var entity = _userRepository.GetByKey(key);

      if (entity == null)
      {
        return NotFound();
      }

      _userRepository.Delete(entity);
      _userRepository.Save();

      return Ok();
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

    private IQueryable<User> QuerySingle(int key)
    {
      var user = _userRepository.GetByKey(key);

      if (user == null)
      {
        return Enumerable.Empty<User>().AsQueryable();
      }

      return new List<User>
      {
        new User
        {
           Id = user.Id,
           Username = user.Name,
           Email = user.Email,
           Profile = user.Profile,
        }
      }.AsQueryable();
    }
  }
}