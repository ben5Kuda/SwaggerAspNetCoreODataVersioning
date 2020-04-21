using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerAspCoreOData.Authorization
{
  /// <summary>
  /// Represents a requirement for a policy
  /// </summary>
  public class AuthorizeRequirement : IAuthorizationRequirement
  {
    public string UserProfile { get; set; }

    public AuthorizeRequirement(string userProfile)
    {
      UserProfile = userProfile;
    }

  }
}
