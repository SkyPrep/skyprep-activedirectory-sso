namespace SkyPrepIntegration.UI.Services
{
    using System.DirectoryServices.AccountManagement;

    public class AdService
    {
        public static string GetEmailAddress()
        {
            return UserPrincipal.Current.EmailAddress;
        }
    }
}