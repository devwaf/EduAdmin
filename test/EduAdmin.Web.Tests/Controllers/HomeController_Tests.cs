using System.Threading.Tasks;
using EduAdmin.Models.TokenAuth;
using EduAdmin.Web.Controllers;
using Shouldly;
using Xunit;

namespace EduAdmin.Web.Tests.Controllers
{
    public class HomeController_Tests: EduAdminWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123456"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}