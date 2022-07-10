using System.Collections.Generic;

namespace Spot.Api.Infrastructure.ModelHelper
{
    /// <summary>
    /// Custom Validation error
    /// </summary>
    public class ValidationError
    {
        public List<Error> Errors;
    }

    /// <summary>
    /// Error message for custom validation
    /// </summary>
    public class Error
    {
        public string Field { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
