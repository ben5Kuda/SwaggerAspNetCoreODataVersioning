using SwaggerAspCoreOData.Models.Examples;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerAspCoreOData.Models
{
  [SwaggerSchemaFilter(typeof(UserSchemaExample))]
  public class User
  {
    [Key]
    public int Id { get; set; }
    public string Username { get; set; }
    public string Profile { get; set; }
    public string Email { get; set; }

    public IEnumerable<Role> Roles { get; set; }
  }

  public class Role
  {
    public int Id { get; set; }   
  }
}
