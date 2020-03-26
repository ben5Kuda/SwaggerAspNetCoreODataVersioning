using SwaggerAspCoreOData.Infastructure;
using SwaggerAspCoreOData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerAspCoreOData.Repositories
{
  public interface IUserRepository : IRepository<DBContext.Users>
  {
    IEnumerable<DBContext.Users> GetUserRoles();
  }
}
