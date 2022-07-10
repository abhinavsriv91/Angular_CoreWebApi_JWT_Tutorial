using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;
using Tutorial.Api.Infrastructure.AttributeHelper;
using Tutorial.Api.Infrastructure.JWT;
using Tutorial.Business.Interfaces;
using Tutorial.Business.Models;
using Tutorial.Global;
using Tutorial.Global.DTO;
using Tutorial.Global.Exceptions;
using Tutorial.Logger;
using Tutorial.Security;

namespace Tutorial.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(TutorialActionFilterAttribute))]
    [ServiceFilter(typeof(TutorialExceptionFilterAttribute))]
    [CustomModelValidation]
    public class AuthenticationController : ControllerBase
    {
        private readonly ITutorialServices _Services;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// Injecting the Dependencies
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public AuthenticationController(ITutorialServices services, IConfiguration configuration, IMemoryCache memoryCache)
        {
            _Services = services;
            _configuration = configuration;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Check and insert Authenticated user in DB
        /// </summary>
        ///<param name="userName"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("user/authenticate")]
        public async Task<IActionResult> AuthenticateUser([FromBody] User requestUser)
        {
            IActionResult response = NoContent();
            try
            {
                AuthenticatedUserDTO user = await _Services.AuthenticateUser(requestUser.UserName, requestUser.Password);
                if (user != null)
                {
                    SetCurrentThread(user);
                    var timeOut = Convert.ToDouble(_configuration.GetSection(GlobalConstants.AppSettings)[GlobalConstants.TimeOut]);
                    var tokenString = JWTManager.GenerateJWTToken(user, _configuration[WebConstants.JwtSecretKey], timeOut);

                    response = Ok(new
                    {
                        code = 0,
                        user.UserName,
                        user.FullName,
                        user.EmailAddress,
                        user.Roles,
                        token = tokenString
                    });
                }
            }
            catch (APILayerException ex)
            {
                response = Ok(new
                {
                    code = -1,
                    message = "The User does not exists."
                });
                TutorialLogger.LogError(requestUser.UserName, ex.Message, ex.StackTrace);
            }
            return response;
        }

        /// <summary>
        /// Insert Current user context in Cache and set the current thread
        /// </summary>
        /// <param name="user"></param>
        private void SetCurrentThread(AuthenticatedUserDTO user)
        {
            _memoryCache.TryGetValue(user.UserName + WebConstants.CustomPrincipal, out CustomPrincipal customPrincipal);
            if (customPrincipal == null)
            {
                customPrincipal = new CustomPrincipal(user);
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromMinutes(Convert.ToDouble(_configuration.GetSection(GlobalConstants.AppSettings)[GlobalConstants.TimeOut])));

                _memoryCache.Set(user.UserName + WebConstants.CustomPrincipal, customPrincipal, cacheEntryOptions);
            }
            HttpContext.User = customPrincipal;
            Thread.CurrentPrincipal = customPrincipal;
        }
    }
}
