namespace Spot.Api.Infrastructure.ModelHelper
{
    /// <summary>
    /// Error Model
    /// </summary>
    public class GlobalError
    {
        /// <summary>
        /// Error Code
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Error Message
        /// </summary>
        public string Message { get; set; }

        public GlobalError()
        {
        }
    }
}
