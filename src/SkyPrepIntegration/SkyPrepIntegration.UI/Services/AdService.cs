namespace SkyPrepIntegration.UI.Services
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.DirectoryServices.AccountManagement;
    using System.Web;

    public class AdService
    {

        private static UserPrincipal CurrentUser;

        public static UserPrincipal GetCurrentUser()
        {
            if (CurrentUser != null)
            {
                return CurrentUser;
            }

            CurrentUser = GetUserFromUserPrincipal();
            if (CurrentUser != null)
            {
                CurrentUser = GetUserFromPrincipalContext();
            }

            return CurrentUser;
        }


        public static UserPrincipal GetUserFromUserPrincipal()
        {
            try
            {
                CurrentUser = UserPrincipal.Current;
                return CurrentUser;


            }
            catch (Exception e)
            {
            }
            return null;
        }

        public static UserPrincipal GetUserFromPrincipalContext()
        {
            try
            {
                var domain = new PrincipalContext(ContextType.Domain);
                var name = HttpContext.Current.User.Identity.Name;
                CurrentUser = UserPrincipal.FindByIdentity(domain, name);
                return CurrentUser;
            }
            catch (Exception e)
            {
            }
            return null;

        }


        public static string GetFirstName()
        {
            if (GetCurrentUser() != null)
            {
                return GetCurrentUser().GivenName;
            } 
            return "";
            
        }

        public static string GetLastName()
        {
            if (GetCurrentUser() != null)
            {
                return GetCurrentUser().Surname;
            }
            return "";
        }
        public static string GetEmailAddress()
        {
            if (GetCurrentUser() != null)
            {
                return GetCurrentUser().EmailAddress;
            }
            return "";
        }

        public static string GetUserUsername()
        {
            if (GetCurrentUser() != null)
            {
                return GetCurrentUser().SamAccountName;
            }
            return "";
        }



        public static List<string> GetUserGroups()
        {
            var results = new List<string>();
            if (GetCurrentUser() != null)
            {
                var user = GetCurrentUser();
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
            var currentUser = GetCurrentUser();
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