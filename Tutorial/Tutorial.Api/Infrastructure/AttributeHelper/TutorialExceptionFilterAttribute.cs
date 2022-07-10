using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Spot.Api.Infrastructure.ModelHelper;
using Tutorial.Global;
using Tutorial.Global.Exceptions;
using Tutorial.Logger;
using Tutorial.Security;
using System;
using System.Threading;

namespace Tutorial.Api.Infrastructure.AttributeHelper
{
    /// <summary>
    /// Exception Filter to be in action in case of Exception in the application
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class TutorialExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;

        public TutorialExceptionFilterAttribute(IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _configuration = configuration;
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// It will be executed in case of any runtime exception
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            if (context != null)
            {
                bool debug = Convert.ToBoolean(_configuration.GetSection(GlobalConstants.AppSettings)[GlobalConstants.Debug]);
                GlobalError error;
                if (context.Exception is TutorialApplicationException)
                {
                    var exception = context.Exception as TutorialApplicationException;
                    if (!debug)
                    {
                        error = new GlobalError()
                        {
                            Code = -1,
                            Message = exception.Message,
                        };
                    }
                    else 
                    {
                        error = new DebugError()
                        {
                            Code = -1,
                            Message = exception.GetBaseException().Message,
                            StackTrace = exception.GetBaseException().StackTrace
                        };
                    }
                    context.HttpContext.Response.StatusCode = exception.StatusCode;
                    context.Result = new JsonResult(error);
                }
                else if (context.Exception is UnauthorizedAccessException)
                {
                    var exception = context.Exception as UnauthorizedAccessException;
                    if (!debug)
                    {
                        error = new GlobalError()
                        {
                            Code = -1,
                            Message = "UnAuthorized request."
                        };
                    }
                    else 
                    {
                        error = new DebugError()
                        {
                            Code = -1,
                            Message = exception.GetBaseException().Message,
                            StackTrace = exception.GetBaseException().StackTrace
                        };
                    }
                    context.HttpContext.Response.StatusCode = 401;
                    context.Result = new JsonResult(error);
                }
                else
                {
                    if (!debug)
                    {
                        error = new GlobalError()
                        {
                            Code = -1,
                            Message = "Unhandled Error"
                        };
                    }
                    else
                    {
                        error = new DebugError()
                        {
                            Code = -1,
                            Message = context.Exception.GetBaseException().Message,
                            StackTrace = context.Exception.GetBaseException().StackTrace
                        };
                    }
                    context.Result = new JsonResult(error);
                    context.HttpContext.Response.StatusCode = 500;
                }
                // Log error
                Thread.CurrentPrincipal = context.HttpContext.User  as CustomPrincipal;
                TutorialLogger.LogError(CustomPrincipal.GetCurrentUserName(), context.Exception?.GetBaseException().Message, context.Exception?.GetBaseException().StackTrace);
                base.OnException(context);
            }
        }
    }
}
