using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerAspCoreOData.Models
{
  public class User
  {
    [Key]
    public int Id { get; set; }
    public string Username { get; set; }
    public string Profile { get; set; }

    public IEnumerable<Role> Roles { get; set; }
  }

  public class Role
  {
    public int Id { get; set; }
    public string RoleName { get; set; }
  }
}
