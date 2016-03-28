using System.Web.Mvc;

namespace SkyPrepIntegration.UI.Controllers
{
    using Services;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var apiSettings = Apisetting.Get();
            var userData = AdService.GetUserData();
 
            var ssoLinkService = new SsoLinkService(apiSettings);
            var loginLink = ssoLinkService.GenerateSsoLink(userData);

            if (!string.IsNullOrEmpty(loginLink))
            {
                return Redirect(loginLink);
            }

            return View();
        }
    }
}