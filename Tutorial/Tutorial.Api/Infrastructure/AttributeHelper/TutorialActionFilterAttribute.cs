using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using Tutorial.Api.Infrastructure.JWT;
using Tutorial.Global;
using Tutorial.Global.DTO;
using Tutorial.Security;

namespace Tutorial.Api.Infrastructure.AttributeHelper
{
    /// <summary>
    /// Action Filter Attribute class
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class TutorialActionFilterAttribute : ActionFilterAttribute
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor to inject Memory Cache
        /// </summary>
        /// <param name="memoryCache"></param>
        public TutorialActionFilterAttribute(IMemoryCache memoryCache, IHttpContextAccessor contextAccessor, IConfiguration configuration)
        {
            _memoryCache = memoryCache;
            _contextAccessor = contextAccessor;
            _configuration = configuration;
        }

        /// <summary>
        /// This Method gets executed before every action and set current thread.
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Request.Headers.TryGetValue(WebConstants.AutherizationHeader, out var authorizationToken);

            if (!string.IsNullOrWhiteSpace(authorizationToken))
            {
                AuthenticatedUserDTO user = JWTManager.ExtractClaimFromJWT(authorizationToken);

                _memoryCache.TryGetValue(user.UserName + WebConstants.CustomPrincipal, out CustomPrincipal customPrincipal);
                if (customPrincipal == null)
                {
                    customPrincipal = new CustomPrincipal(user);
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                                .SetSlidingExpiration(TimeSpan.FromMinutes(Convert.ToDouble(_configuration.GetSection(GlobalConstants.AppSettings)[GlobalConstants.TimeOut])));

                    _memoryCache.Set(user.UserName + WebConstants.CustomPrincipal, customPrincipal, cacheEntryOptions);
                }
                Thread.CurrentPrincipal = customPrincipal;
                _contextAccessor.HttpContext.User = customPrincipal;
            }
        }

        /// <summary>
        /// This Method gets executed after every action.
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}
