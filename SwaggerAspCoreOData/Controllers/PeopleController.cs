using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SwaggerAspCoreOData.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace SwaggerAspCoreOData.Controllers
{

  [ApiVersion("1.0")]
  [ApiVersion("2.0")]
  [SwaggerTag("Provides operations to manage people.")]
  public class PeopleController : ODataController
  {
    [HttpGet]
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
