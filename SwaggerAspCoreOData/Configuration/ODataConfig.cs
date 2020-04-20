using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Mvc;
using SwaggerAspCoreOData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerAspCoreOData.Configuration
{

  /// <summary>
  /// Represents the model configuration for OData.
  /// </summary>
  public class ODataConfig : IModelConfiguration
  {
    /// <summary>
    /// Applies model configurations using the provided builder for the specified API version.
    /// </summary>
    /// <param name="builder">The <see cref="ODataModelBuilder">builder</see> used to apply configurations.</param>
    /// <param name="apiVersion">The <see cref="ApiVersion">API version</see> associated with the <paramref name="builder"/>.</param>
    public void Apply(ODataModelBuilder builder, ApiVersion apiVersion)
    {
      var person = builder.EntitySet<Person>("People").EntityType;
      person.HasKey(p => p.Id);

      var user = builder.EntitySet<User>("Users").EntityType;
      user.HasKey(p => p.Id);

      var personApiKeyAuth = builder.EntitySet<Person>("PeopleApiKeyAuth").EntityType;
      user.HasKey(p => p.Id);

      if (apiVersion < ApiVersions.V3)
      {
        user.Ignore(p => p.Profile);
      }
    }
  }
}
