using Microsoft.EntityFrameworkCore;
using SwaggerAspCoreOData.Infastructure;
using SwaggerAspCoreOData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerAspCoreOData.Repositories
{
  public class UserRepository : Repository<DBContext.Users>, IUserRepository
  {
    private DBContext.SampleDbContext _sampleDbContext;
    public UserRepository(DBContext.SampleDbContext sampleDbContext) : base(sampleDbContext)
    {
      _sampleDbContext = sampleDbContext;
    }

    public IEnumerable<DBContext.Users> GetUserRoles()
    {
      return _sampleDbContext.Users.Include(ur => ur.UserRoles);        
    }
  }
}
