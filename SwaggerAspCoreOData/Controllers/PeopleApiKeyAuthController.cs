using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwaggerAspCoreOData.Authentication;
using SwaggerAspCoreOData.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace SwaggerAspCoreOData.Controllers
{

  [ApiVersion("1.0")]
  [ApiVersion("2.0")]
  [ApiVersion("3.0")]
  [Produces("application/json")]
  [SwaggerTag("Provides operations to test Api Key Authentication.")]
  public class PeopleApiKeyAuthController : ODataController
  {
    [HttpGet]
    [ApiKeyAuth] // This attribute enables forces the Controller to be authenticated by an Api Key
    [EnableQuery]
    public IQueryable<Person> Get()
    {
        return new List<Person>
        {
            new Person
            {
                Id = 10,
                Name = "Kuda"
            }
        }.AsQueryable();
        }
  }
}
