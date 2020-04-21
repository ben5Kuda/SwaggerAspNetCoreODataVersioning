using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using SwaggerAspCoreOData.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SwaggerAspCoreOData.Authorization
{

  /// <summary>
  /// Represents operations to handle policies based on a given requirement(s)
  /// </summary>
  public class AuthorizeHandler : AuthorizationHandler<AuthorizeRequirement>
  {
    private readonly IUserRepository _userRepository;
    /// <summary>
    /// Initialises the handler
    /// </summary>
    public AuthorizeHandler(IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    /// <summary>
    /// Handles the registered policy requirements
    /// </summary>
    /// <param name="context"></param>
    /// <param name="requirement"></param>
    /// <returns></returns>
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizeRequirement requirement)
    {
      if (!context.User.Identity.IsAuthenticated)
      {
        context.Fail();
        return Task.CompletedTask;
      }

      if (!context.User.HasClaim(c => c.Type == JwtClaimNames.Sub))
      {
        context.Fail();
        return Task.CompletedTask;
      }

      var request = ((Microsoft.AspNetCore.Mvc.ActionContext)context.Resource).HttpContext.Request;
      string action = request.Method;

      bool isAuthorized = CheckAccess(context.User, request, action, requirement) ? true : false;

      if (!isAuthorized)
      {
        context.Fail();
        return Task.CompletedTask;
      }

      context.Succeed(requirement);
      return Task.CompletedTask;
    }

    // Additional checks can be perfomed against the request/resource and the action
    private bool CheckAccess(ClaimsPrincipal identity, HttpRequest request, string action, AuthorizeRequirement requirement)
    {
      int userId;
      int.TryParse(identity.FindFirst(JwtClaimNames.Sub)?.Value, out userId);
      var user = _userRepository.GetByKey(userId);

      if (user == null)
      {
        return false;
      }

      // validate against the requirement (the user profile must be Admin as defined by the policy)
      if (user.Profile != requirement.UserProfile)
      {
        return false;
      }

      return true;
    }
  }
  }
