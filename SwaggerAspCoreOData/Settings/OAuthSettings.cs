using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerAspCoreOData.Settings
{
  public class OAuthSettings
  {
    public static string STSAuthority { get; set; }
    public static string STSAudience { get; set; }
    public static string SwaggerAuthorizationUrl { get; set; }
    public static string SwaggerClientSecret { get; set; }
    public static string SwaggerClientId { get; set; }

  }
}
