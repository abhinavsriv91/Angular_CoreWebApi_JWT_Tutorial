using Microsoft.AspNetCore.Mvc.Filters;
using Tutorial.Api.Infrastructure.JWT;
using Tutorial.Business.Interfaces;
using Tutorial.Global.DTO;
using Tutorial.Global.Exceptions;
using Tutorial.Logger;
using Tutorial.Security;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Tutorial.Api.Infrastructure.AttributeHelper
{
    /// <summary>
    /// Filter to handle authorization of every request.
    /// </summary>
    public class TutorialRoleAuthorizationFilter : IAsyncAuthorizationFilter
    {
        public string AllowedRoles { get; set; }
        private readonly ITutorialServices _Services;

        public TutorialRoleAuthorizationFilter(ITutorialServices services)
        {
            _Services = services;
        }

        /// <summary>
        /// Check authorization of the incoming request 
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        public async Task OnAuthorizationAsync(AuthorizationFilterContext filterContext)
        {
            filterContext.HttpContext.Request.Headers.TryGetValue(WebConstants.AutherizationHeader, out var authorizationToken);
            bool authorize = false;

            if (!string.IsNullOrWhiteSpace(authorizationToken))
            {
                if (AllowedRoles != "All")
                {
                    string[] allowedRolesArray = AllowedRoles.Split(',');
                    AuthenticatedUserDTO user = JWTManager.ExtractClaimFromJWT(authorizationToken);
                    IList<string> availableRoles = await _Services.SelectRoles(user.UserName);
                    foreach (var role in allowedRolesArray)
                    {
                        if (availableRoles.Contains(role))
                        {
                            authorize = true;
                            break;
                        }
                    }
                }
                else if (AllowedRoles == "All")
                    authorize = true;
                else
                    authorize = false;
                if (!authorize) 
                {
                    string message = "Current request is not authorized for this call.";
                    TutorialLogger.LogError(CustomPrincipal.GetCurrentUserName(), message, null);
                    throw new UnAuthorizedException(message, (int)HttpStatusCode.Forbidden, null);
                }
            }
        }
    }
}
