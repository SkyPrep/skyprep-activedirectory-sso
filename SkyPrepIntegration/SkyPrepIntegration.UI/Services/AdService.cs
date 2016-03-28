namespace SkyPrepIntegration.UI.Services
{
    using System.DirectoryServices.AccountManagement;

    public class AdService
    {
        public static string GetFirstName()
        {
            return UserPrincipal.Current.GivenName;
        }

        public static string GetLastName()
        {
            return UserPrincipal.Current.Surname;
        }
        public static string GetEmailAddress()
        {
            return UserPrincipal.Current.EmailAddress;
        }

        public static string GetUserGroups()
        {
            return "";
        }
    }
}