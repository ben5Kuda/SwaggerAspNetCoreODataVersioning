using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SwaggerAspCoreOData.Settings;
using Swashbuckle.AspNetCore.Annotations;

namespace SwaggerAspCoreOData.Controllers
{

  [Produces("application/json")]
  [SwaggerTag("Provides operations to manage environments.")]
  public class EnvironmentController : ODataController
  {
    private readonly IOptions<CurrentEnviroment> _settings;

    public EnvironmentController(IOptions<CurrentEnviroment> settings)
    {
      _settings = settings;
    }

    [HttpGet]
    public IEnumerable<string> Get()
    {
      return new string[] { "This service is up and running..." + " on " + Environment.MachineName + ", Stage: " + _settings.Value.Stage };
    }
  }
}
