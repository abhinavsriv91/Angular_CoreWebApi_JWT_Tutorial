using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Tutorial.Business.Interfaces;
using Tutorial.Data.Repository;
using Tutorial.IOC;

namespace Tutorial.Tests.Business
{
    [TestFixture]
    public class TutorialServiceTests
    {
        private ITutorialServices _spotServices;
        private Mock<ITutorialRepo> _spotRepoMock;
        private IMapper _mapper;
        private IConfiguration _configuration;

        public TutorialServiceTests()
        {
            _spotRepoMock = new Mock<ITutorialRepo>();
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton(Configuration);

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfiles());
            });
            _mapper = mappingConfig.CreateMapper();
        }

        public IConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    var builder = new ConfigurationBuilder().AddJsonFile($"testsettings.json", optional: false);
                    _configuration = builder.Build();
                }

                return _configuration;
            }
        }

        [SetUp]
        public void Setup()
        {
        }
    }
}
