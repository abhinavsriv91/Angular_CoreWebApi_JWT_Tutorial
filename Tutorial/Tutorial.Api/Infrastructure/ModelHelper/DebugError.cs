namespace Spot.Api.Infrastructure.ModelHelper
{

    /// <summary>
    /// Error model to bind in case of debug
    /// </summary>
    public class DebugError : GlobalError
    {
        /// <summary>
        /// Stack trace
        /// </summary>
        public string StackTrace { get; set; }
    }
}
