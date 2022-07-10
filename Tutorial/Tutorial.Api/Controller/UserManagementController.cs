using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Tutorial.Api.Infrastructure.AttributeHelper;
using Tutorial.Business.Interfaces;
using Tutorial.Business.Models;
using Tutorial.Global.Exceptions;
using Tutorial.Logger;

namespace Tutorial.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {

        private readonly ITutorialServices _Services;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// Injecting the Dependencies
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public UserManagementController(ITutorialServices services, IConfiguration configuration, IMemoryCache memoryCache)
        {
            _Services = services;
            _configuration = configuration;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Add User in DB
        /// </summary>
        ///<param name="userName"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("user/add")]
        [TutorialRoleAuthorizationFilterFactory(AllowedRoles = "Admin")]
        public async Task<IActionResult> AddUser([FromBody] User requestUser)
        {
            IActionResult response = NoContent();
            try
            {
                int returnCode = await _Services.AddUser(requestUser);
                if (returnCode > 0)
                {
                    response = Ok(new
                    {
                        code = 0,
                        Message = "User added successfully."
                    });
                }
            }
            catch (APILayerException ex)
            {
                response = Ok(new
                {
                    code = -1,
                    message = "Error in adding new user."
                });
                TutorialLogger.LogError(requestUser.UserName, ex.Message, ex.StackTrace);
            }
            return response;
        }
    }
}
