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
            var ssoService = new SsoLinkService();

            //act
            var ssoResponse = ssoService.GenerateSsoLink();

            //assert
            ssoResponse.ShouldBeNullOrEmpty();


        }
    }
}
