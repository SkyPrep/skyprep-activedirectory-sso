using System.Web.Mvc;

namespace SkyPrepIntegration.UI.Controllers
{
    using Services;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var apiSettings = Apisetting.Get();
            var emailAddress = AdService.GetEmailAddress();
            var firstName = AdService.GetFirstName();
            var lastName = AdService.GetLastName();
            var uGroups = AdService.GetUserGroups();
            
            var ssoLinkService = new SsoLinkService(apiSettings);
            var loginLink = ssoLinkService.GenerateSsoLink(emailAddress, firstName, lastName, uGroups);

            if (!string.IsNullOrEmpty(loginLink))
            {
                return Redirect(loginLink);
            }

            return View();
        }
    }
}