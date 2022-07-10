using Spot.Api.Infrastructure.ModelHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Tutorial.Api.Infrastructure.AttributeHelper
{
    /// <summary>
    /// Action filter for custom handling model validation.
    /// </summary>
    public class CustomModelValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.ErrorCount > 0)
            {
                var validationError = new ValidationError()
                {
                    Errors = new List<Error>()
                };

                var modelType = context.ActionDescriptor.Parameters
                    .FirstOrDefault(prop => prop.BindingInfo.BindingSource.Id.Equals("Body", StringComparison.InvariantCultureIgnoreCase))?.ParameterType;

                if (modelType == null)
                {
                    modelType = context.ActionDescriptor.Parameters
                    .FirstOrDefault(prop => prop.BindingInfo.BindingSource.Id.Equals("Path", StringComparison.InvariantCultureIgnoreCase))?.ParameterType;
                }

                foreach (var item in context.ModelState)
                {
                    var property = modelType.GetProperties().FirstOrDefault(prop => prop.Name.Equals(item.Key, StringComparison.InvariantCultureIgnoreCase));
                    string propertyName = property != null ? property.Name : item.Key;
                    if (property != null)
                    {
                        var displayNameAttributeValue = property.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().SingleOrDefault()?.DisplayName;
                        propertyName = displayNameAttributeValue ?? propertyName;
                    }

                    // Add only "Is required" error if its available.
                    if (item.Value.Errors.Any(x => x.ErrorMessage.Contains("is required")))
                    {
                        validationError.Errors.Add(new Error
                        {
                            Field = propertyName,
                            ErrorMessages = new List<string> { item.Value.Errors.First().ErrorMessage }
                        });
                    }
                    else
                    {
                        validationError.Errors.Add(new Error
                        {
                            Field = propertyName,
                            ErrorMessages = item.Value.Errors.Select(error => error.ErrorMessage).ToList()
                        });
                    }
                }
                context.Result = new BadRequestObjectResult(validationError);
            }
            base.OnActionExecuting(context);
        }
    }
}
