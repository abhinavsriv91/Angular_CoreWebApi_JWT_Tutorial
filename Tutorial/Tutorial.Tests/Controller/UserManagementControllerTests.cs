using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Tutorial.Api.Controller;
using Tutorial.Business.Interfaces;
using Tutorial.Business.Models;

namespace Tutorial.Tests.Controller
{

    [TestFixture]
    public class UserManagementControllerTests
    {
        private Mock<ITutorialServices> tutorialServiceMock;
        private IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;
        private UserManagementController _managementController;

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

        public UserManagementControllerTests()
        {
            tutorialServiceMock = new Mock<ITutorialServices>();

            IServiceCollection services = new ServiceCollection();
            services.AddSingleton(Configuration);
            services.AddMemoryCache();

            var serviceProvider = services.BuildServiceProvider();
            _memoryCache = serviceProvider.GetService<IMemoryCache>();
        }

        [Test]
        public async Task Test_AddComputerTypes_Success()
        {
            // Arrange
            string userName = "srivait";

            User userModel = new User()
            {
                UserName = userName,
                Password = "password",
                EmailAddress = "a@a.com",
                FullName = "ABC ABC",
                Roles = new List<string>() { "Manager", "Edit" }
            };

            tutorialServiceMock.Setup(x => x.AddUser(userModel)).ReturnsAsync(1);

            // Act
            _managementController = new UserManagementController(tutorialServiceMock.Object, null, null);
            var response = await _managementController.AddUser(userModel) as ObjectResult;
            var content = response.Value;

            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, 200);

            Type type = content.GetType();
            PropertyInfo[] fields = type.GetProperties();
            foreach (var field in fields)
            {
                string name = field.Name;
                var value = field.GetValue(content, null);

                if (name == "code")
                    Assert.AreEqual(1, value);
                if (name.ToLower() == "message")
                    Assert.AreEqual("Success", value);
            }
        }
    }
}
