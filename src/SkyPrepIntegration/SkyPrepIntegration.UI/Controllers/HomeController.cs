using System.Web.Mvc;

namespace SkyPrepIntegration.UI.Controllers
{
    using Services;
    using System.Diagnostics;
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var apiSettings = Apisetting.Get();
            string emailAddress = AdService.GetEmailAddress();
            string firstName = AdService.GetFirstName();
            string lastName = AdService.GetLastName();
            string uGroups = SkyPrepHelpers.ListToString(AdService.GetUserGroupsAlternate(), ";");
            
            var ssoLinkService = new SsoLinkService(apiSettings);
            Debug.WriteLine("User Groups List:");
            Debug.WriteLine(uGroups);
            var loginLink = ssoLinkService.GenerateSsoLink(emailAddress, firstName, lastName, uGroups);

            if (!string.IsNullOrEmpty(loginLink))
            {
                return Redirect(loginLink);
            }

            return View();
        }
    }
}