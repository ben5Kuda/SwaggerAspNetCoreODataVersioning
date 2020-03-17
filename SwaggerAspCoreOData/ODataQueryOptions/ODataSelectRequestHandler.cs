using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwaggerAspCoreOData.ODataQueryOptions
{
  /// <summary>
  /// Providers operations to apply defaults to selected requests
  /// </summary>
  public class ODataSelectRequestHandler : ActionFilterAttribute
  {
    /// <summary>
    /// Applies defaults to odata requests and only selects the top level properties only, if none specified
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
      var requestContext = context.HttpContext.Request;

      //check if request contains get, if not exit
      if (requestContext.Method != "GET")
      {
        return base.OnActionExecutionAsync(context, next);
      }

      var query = requestContext.Query;

      if (query.Any())
      {
        var keys = query.Keys;

        if (keys.Count > 0)
        {
          if (keys.Contains("$select"))
          {
            //top level select, exit
            return base.OnActionExecutionAsync(context, next);
          }
        }
      }

      // get the declared properties on the given model
      var atrributeInfo = (Microsoft.AspNet.OData.Routing.ODataAttributeRouteInfo)context.ActionDescriptor.AttributeRouteInfo;
      var odataPathSegments = atrributeInfo.ODataTemplate.Segments;
      var entitySetSegments = odataPathSegments.Select(x => (Microsoft.AspNet.OData.Routing.Template.EntitySetSegmentTemplate)x);
      var edmCollectionTypes = entitySetSegments.Select(x => (Microsoft.OData.Edm.EdmCollectionType)x.Segment.EdmType);
      var edmTypeReferences = edmCollectionTypes.Select(x => (Microsoft.OData.Edm.EdmTypeReference)x.ElementType);
      var edmTypes = edmTypeReferences.Select(x => x.Definition);
      var edmStructuredType = edmTypes.Select(x => (Microsoft.OData.Edm.EdmStructuredType)x).FirstOrDefault();
      var model = edmStructuredType.DeclaredProperties.ToList();

      //for every property on the model, apply odata select to top level/non navigational properties
      StringBuilder modelBuilder = new StringBuilder();
      for (int i = 0; i < model.Count(); i++)
      {
        if (model[i].PropertyKind != Microsoft.OData.Edm.EdmPropertyKind.Navigation)
        {
          if (modelBuilder.Length != 0)
          {
            modelBuilder.Append(",");
          }

          modelBuilder.Append(model[i].Name.ToCamelCase());
        }
      }

      if (modelBuilder.Length == 0)
      {
        return base.OnActionExecutionAsync(context, next);
      }

      var selectQuery = "$select";

      //modify the odata query
      context.HttpContext.Request.QueryString = requestContext.QueryString.Add(selectQuery, modelBuilder.ToString());

      return base.OnActionExecutionAsync(context, next);
    }
  }

  /// <summary>
  /// Provides operations to convert to camelCase
  /// </summary>
  public static class CamelCaseHelper
  {

    /// <summary>
    /// Modifies a given string and returns camelCase version of the string.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string ToCamelCase(this string s)
    {
      if (s == null)
      {
        throw new ArgumentNullException(nameof(s));
      }

      if (s.Length != 0)
      {
        return s[0].ToString().ToLower() + s.Substring(1);
      }

      return s;
    }
  }
}
