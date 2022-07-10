using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
using System.Text;
using Tutorial.Api.Infrastructure.AttributeHelper;
using Tutorial.Global;
using Tutorial.Global.Exceptions;
using Tutorial.IOC;

namespace Tutorial.Api
{
    public class Startup
    {
        readonly string allowOrigin = "AllowOrigin";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(option =>
            {
                option.AddPolicy(name: allowOrigin,
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200")
                               .AllowAnyHeader()
                               .AllowAnyMethod()
                               .AllowCredentials();
                    });
            });

            services.AddMemoryCache();
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(Convert.ToDouble(Configuration.GetSection(GlobalConstants.AppSettings)[GlobalConstants.TimeOut]));
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddControllers().AddNewtonsoftJson(s =>
            {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddScoped<TutorialActionFilterAttribute>();
            services.AddScoped<TutorialExceptionFilterAttribute>();
            services.AddScoped<TutorialRoleAuthorizationFilter>();

            services.AddHttpContextAccessor();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                     options =>
                     {
                         options.RequireHttpsMetadata = false;
                         options.SaveToken = true;
                         options.TokenValidationParameters = new TokenValidationParameters
                         {
                             ValidateIssuer = false,
                             ValidateAudience = false,
                             ValidateLifetime = true,
                             ValidateIssuerSigningKey = true,
                             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"])),
                             ClockSkew = TimeSpan.Zero
                         };
                     });

            // setting to preserve the http header in case of proxy and Load balancer.
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            // Surpress default Model Validation
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseForwardedHeaders();

            app.UseHttpsRedirection();

            // global cors policy
            //app.UseCors(x => x.AllowAnyOrigin()
            //                  .AllowAnyMethod()
            //                  .AllowAnyHeader());

            app.UseRouting();
            app.UseCors(allowOrigin);
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseExceptionHandler(app =>
            {
                app.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                    if (ex != null && ex.GetType() == typeof(UnAuthorizedException))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    }
                    await context.Response.WriteAsync(ex.Message);
                });
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        /// <summary>
        /// Injecting all dependencies of the Application to Middleware
        /// </summary>
        /// <param name="services"></param>
        private void RegisterServices(IServiceCollection services)
        {
            DependencyContainer.RegisterServices(services);
        }
    }
}
