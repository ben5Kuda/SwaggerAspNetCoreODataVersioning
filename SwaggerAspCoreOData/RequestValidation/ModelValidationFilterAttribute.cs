using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwaggerAspCoreOData.RequestValidation
{

  /// <summary>
  /// Represents a request validation handler, mostly targeting bad requests
  /// </summary>
  public class ModelValidationFilterAttribute : IActionFilter
  {
    /// <summary>
    /// Handles invalid requests and short circuits the request, if any errors are present in the model state
    /// </summary>
    /// <param name="context"></param>
    public void OnActionExecuting(ActionExecutingContext context)
    {

      if (!context.ModelState.IsValid)
      {
        bool isValid = false;
        var modelErrors = new List<Microsoft.AspNetCore.Mvc.ModelBinding.ModelError>(context.ModelState.Root.Errors).ToArray();
        string modelErrorMessage = string.Empty;

        if (modelErrors.Any())
        {
          isValid = true;
          modelErrorMessage = modelErrors[0].Exception.Message;
          context.Result = new BadRequestObjectResult(modelErrorMessage);
        }

        var modelPropertyStateErrors = new List<Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateEntry>(context.ModelState.Values).ToArray();
        if (modelPropertyStateErrors.Any())
        {
          var propertyStateError = modelPropertyStateErrors[0].Errors[0].ErrorMessage;
          context.Result = isValid ? new BadRequestObjectResult(modelErrorMessage) : new BadRequestObjectResult(propertyStateError);
        }

      }
    }

    /// <summary>
    /// Handles requests after they have been executed by the action
    /// </summary>
    /// <param name="context"></param>
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
  }
}
