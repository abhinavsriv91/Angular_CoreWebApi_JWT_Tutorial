using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Tutorial.Business.Interfaces;
using Tutorial.Business.Services;
using Tutorial.Data;
using Tutorial.Data.Repository;
using Tutorial.Logger;

namespace Tutorial.IOC
{
    /// <summary>
    /// It holds all the dependencies of the Application and inject them to the MiddleWare
    /// </summary>
    public class DependencyContainer
    {
        /// <summary>
        /// Add all the dependencies to Service Collection
        /// </summary>
        /// <param name="services"></param>
        public static void RegisterServices(IServiceCollection services)
        {
            // Engine Services
            services.AddTransient<ITutorialServices, TutorialServices>();

            // Data
            services.AddTransient<ITutorialRepo, SqlTutorialRepo>();
            services.AddTransient<ISqlManager, SqlManager>();

            // Auto Mapper
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfiles());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            mappingConfig.AssertConfigurationIsValid();
            services.AddSingleton(mapper);

            //Logger
            services.AddScoped<TutorialLogger>();
        }
    }
}
