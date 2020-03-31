using Microsoft.OpenApi;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerAspCoreOData.Models.Examples
{
  public class UserSchemaExample : ISchemaFilter
  {
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {     
 
       schema.Example = new OpenApiObject
       {
         ["id"] = new OpenApiInteger(1),
         ["username"] = new OpenApiString("Kuda"),
         ["email"] = new OpenApiString("kmkuda@me.com"),
         ["profile"] = new OpenApiString("Everyone"),
         ["roles"] = new OpenApiArray()
      };
    }  
  }
}
