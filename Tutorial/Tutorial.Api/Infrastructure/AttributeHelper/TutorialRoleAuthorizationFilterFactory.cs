using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Tutorial.Api.Infrastructure.AttributeHelper
{
    /// <summary>
    /// Action filter factory for Role Authorization
    /// </summary>
    public class TutorialRoleAuthorizationFilterFactoryAttribute : ActionFilterAttribute, IFilterFactory
    {
        public string AllowedRoles { get; set; }

        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            var filter = serviceProvider.GetService<TutorialRoleAuthorizationFilter>();
            if (AllowedRoles != null)
            {
                filter.AllowedRoles = AllowedRoles;
            }
            return filter;
        }
    }
}
