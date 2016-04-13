namespace SkyPrepIntegration.UI.Services
{
    using System;
    using System.DirectoryServices.AccountManagement;
    using System.Web;

    public class AdService


    {

        public static UserPrincipal getCurrentUser()
        {
            try
            {
                return UserPrincipal.Current;
                

            } catch (Exception e)
            {
                try
                {
                    var domain = new PrincipalContext(ContextType.Domain);
                    var name = HttpContext.Current.User.Identity.Name;
                    return UserPrincipal.FindByIdentity(domain, name);
                }   catch (Exception ee)
                {
                    
                }
            }
            return null;
        } 
        public static string GetFirstName()
        {
            return getCurrentUser().GivenName;
        }

        public static string GetLastName()
        {
            return getCurrentUser().Surname;
        }
        public static string GetEmailAddress()
        {
            return getCurrentUser().EmailAddress;
        }

        public static string GetUserGroups()
        {
            return "";
        }
    }
}