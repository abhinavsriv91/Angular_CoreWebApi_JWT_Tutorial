using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using Tutorial.Data;
using Tutorial.Data.Repository;

namespace Tutorial.Tests.Data
{
    [TestFixture]
    public class SqlSpotRepoTests
    {
        private SqlTutorialRepo _spotRepo;
        private IConfiguration _configuration;
        private Mock<ISqlManager> _dataManagerMock;

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

        public SqlSpotRepoTests()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton(Configuration);

            _dataManagerMock = new Mock<ISqlManager>();
            _spotRepo = new SqlTutorialRepo(Configuration, _dataManagerMock.Object);
        }
    }
}
