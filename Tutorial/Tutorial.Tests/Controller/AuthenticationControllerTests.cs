using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tutorial.Api.Controllers;
using Tutorial.Business.Interfaces;
using Tutorial.Business.Models;
using Tutorial.Global.DTO;

namespace Tutorial.Tests.Controller
{
    [TestFixture]
    public class AuthenticationControllerTests
    {
        private Mock<ITutorialServices> tutorialServiceMock;
        private IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;
        private AuthenticationController _authenticationController;

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

        public AuthenticationControllerTests()
        {
            tutorialServiceMock = new Mock<ITutorialServices>();

            IServiceCollection services = new ServiceCollection();
            services.AddSingleton(Configuration);
            services.AddMemoryCache();

            var serviceProvider = services.BuildServiceProvider();
            _memoryCache = serviceProvider.GetService<IMemoryCache>();
        }

        [Test]
        public async Task Test_CheckAuthentication_With_User_Record()
        {
            // Arrange
            string userName = "srivait";

            User userModel = new User()
            {
                UserName = userName,
                Password = "password"
            };

            var json = JsonConvert.SerializeObject(userModel);
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var httpContext = new DefaultHttpContext()
            {
                Request = { Body = stream, ContentLength = stream.Length, Method = HttpMethods.Post },
                Connection = { RemoteIpAddress = IPAddress.Parse("127.0.0.1") }
            };
            var controllerContext = new ControllerContext { HttpContext = httpContext };

            AuthenticatedUserDTO authenticatedUserDTO = new AuthenticatedUserDTO()
            {
                UserName = userName,
                FullName = "Abc abc",
                EmailAddress = "a@a.com",
            };

            tutorialServiceMock.Setup(x => x.AuthenticateUser(userName, userModel.Password)).ReturnsAsync(authenticatedUserDTO);

            // Act
            _authenticationController = new AuthenticationController(tutorialServiceMock.Object, _configuration, _memoryCache)
            {
                ControllerContext = controllerContext
            };
            var response = await _authenticationController.AuthenticateUser(userModel) as ObjectResult;
            dynamic content = response.Value;

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, 200);

            Type type = content.GetType();
            PropertyInfo[] fields = type.GetProperties();
            foreach (var field in fields)
            {
                string name = field.Name;
                var value = field.GetValue(content, null);
                if (name.ToLower() == "returncode")
                    Assert.AreEqual(value, 0);
                else if (name.ToLower() == "fullname")
                    Assert.AreEqual(value, authenticatedUserDTO.FullName);
                else if (name.ToLower() == "username")
                    Assert.AreEqual(value, authenticatedUserDTO.UserName);
                else if (name.ToLower() == "emailaddress")
                    Assert.AreEqual(value, authenticatedUserDTO.EmailAddress);
                else if (name.ToLower() == "token")
                    Assert.IsNotEmpty(value);
            }
        }
    }
}
