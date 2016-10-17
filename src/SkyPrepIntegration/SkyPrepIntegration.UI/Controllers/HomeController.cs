using System.Web.Mvc;

namespace SkyPrepIntegration.UI.Controllers
{
    using Services;
    using System.Diagnostics;
    using System.Collections.Generic;
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var errors = new List<string>();
            var apiSettings = Apisetting.Get();

            var currentUser = AdService.GetCurrentUser();

            if (currentUser == null)
            {
                errors.Add("Could not get Windows users");
            }

            string username = AdService.GetUserUsername();
            if (string.IsNullOrWhiteSpace(username))
            {
                errors.Add("Username is blank");
            }
            ViewData["username"] = username;

            string emailAddress = AdService.GetEmailAddress();
            string firstName = AdService.GetFirstName();
            string lastName = AdService.GetLastName();
            string uGroups = SkyPrepHelpers.ListToString(AdService.GetUserGroupsAlternate(), ";");

            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                errors.Add("Users email is blank"); 
            }
            
            var ssoLinkService = new SsoLinkService(apiSettings);
            Debug.WriteLine("User Groups List:");
            Debug.WriteLine(uGroups);
            var loginLink = ssoLinkService.GenerateSsoLink(emailAddress, firstName, lastName, uGroups);
            Debug.WriteLine(loginLink);
            if (string.IsNullOrWhiteSpace(loginLink))
            {
                errors.Add("Redirect URL is blank");
            }

            if (errors.Count > 0)
            {
                ViewData["errors"] = SkyPrepHelpers.ListToString(errors, ", ");
                return View();
            }

            return Redirect(loginLink);

        }
    }
}