namespace SkyPrepIntegration.UI.Services
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
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
            if (getCurrentUser() != null)
            {
                return getCurrentUser().GivenName;
            } 
            return "";
            
        }

        public static string GetLastName()
        {
            if (getCurrentUser() != null)
            {
                return getCurrentUser().Surname;
            }
            return "";
        }
        public static string GetEmailAddress()
        {
            if (getCurrentUser() != null)
            {
                return getCurrentUser().EmailAddress;
            }
            return "";
        }



        public static List<string> GetUserGroups()
        {
            var results = new List<string>();
            if (getCurrentUser() != null)
            {
                var user = getCurrentUser();
                PrincipalSearchResult<Principal> groups = user.GetAuthorizationGroups();
                foreach(Principal p in groups)
                {
                    if (p is GroupPrincipal)
                    {
                        var group = (GroupPrincipal) p;
                        results.Add(group.Name);
                    }
                }
                return results;
            }
            return null;
        }

        public static List<string> GetUserGroupsAlternate()
        {
            var currentUser = getCurrentUser();
            var rawGroupNames = new List<string>();
            var groups = currentUser.GetGroups();
            var iterGroup = groups.GetEnumerator();
            using (iterGroup)
            {
                while (iterGroup.MoveNext())
                {
                    try
                    {
                        Principal p = iterGroup.Current;
                        rawGroupNames.Add(p.Name);
                    }
                    catch (NoMatchingPrincipalException pex)
                    {
                    }
                }
            }
            return rawGroupNames;
        }


    }
}