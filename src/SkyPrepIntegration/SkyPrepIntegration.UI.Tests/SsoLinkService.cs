namespace SkyPrepIntegration.UI.Tests
{
    using NUnit.Framework;
    using Services;
    using Shouldly;

    public class SsoLinkServiceTest
    {
        [Test]
        public void GivenAllValidInfomration_WhenSkyPrepApiCalled_ShouldReturnAValidUrlInResponse()
        {
            //arrange
            var apiSettings = Apisetting.Get();
            var ssoService = new SsoLinkService(apiSettings);

            //act
            var ssoResponse = ssoService.GenerateSsoLink("arash.barkhodaee@gmail.com", "Arash", "Barkhodaee", "");

            //assert
            ssoResponse.ShouldNotBeEmpty("login url is not avilable.");


        }
    }
}
