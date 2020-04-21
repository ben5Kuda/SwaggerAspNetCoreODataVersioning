using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerAspCoreOData.Authorization
{
  /// <summary>
  /// Represents a set of json web token claims
  /// </summary>
  public static class JwtClaimNames
  {
    /// <summary>
    /// The subject of the jwt, the user
    /// </summary>
    public const string Sub = "sub";

    /// <summary>
    /// The unique name of the subject
    /// </summary>
    public const string UniqueName = "unique_name";

    /// <summary>
    /// The email of the subject
    /// </summary>
    public const string Email = "email";

    /// <summary>
    /// The name of the subject
    /// </summary>
    public const string Name = "name";
   }
 }
