
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerAspCoreOData.Models.Mappers
{
  public class UsersMapper
  {
    /// <summary>
    /// Maps a database entity to a domain model 
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public User FromEntity(DBContext.Users source)
    {
      var target = new User();
      FromEntity(source, target);
      return target;
    }

    /// <summary>
    /// Maps a User domain model to a User database entity
    /// </summary>
    /// <param name="source"></param>
    /// <param name="target"></param>
    public void FromEntity(DBContext.Users source, User target)
    {
      if (source == null)
      {
        throw new ArgumentNullException(nameof(source));
      }

      if (target == null)
      {
        throw new ArgumentNullException(nameof(target));
      }

      target.Id = source.Id;
      target.Username = source.Name;
      target.Profile = source.Profile;
      target.Email = source.Email;      
    }

    public DBContext.Users ToEntity(User source)
    {
      var target = new DBContext.Users();
      ToEntity(source, target);
      return target;
    }

    /// <summary>
    /// Maps a User database entity to a User domain model
    /// </summary>
    /// <param name="source"></param>
    /// <param name="target"></param>
    public DBContext.Users ToEntity(User source, DBContext.Users target)
    {
      if (source == null)
      {
        throw new ArgumentNullException(nameof(source));
      }

      if (target == null)
      {
        throw new ArgumentNullException(nameof(target));
      }

      target.Id = source.Id;
      target.Name = source.Username;
      target.Email = source.Email;
      target.Profile = source.Profile;     

      return target;
    }
  }
}
