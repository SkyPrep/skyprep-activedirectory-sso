using System;
using System.Collections.Generic;
using System.Linq;
using SkyPrepIntegration.UI.Models;

namespace SkyPrepIntegration.UI.Services
{
    using System.DirectoryServices.AccountManagement;

    public class AdService
    {
        public static UserData GetUserData()
        {
            var currentUser = UserPrincipal.Current;
            var groupNames = GetUserGroups(currentUser);

            return new UserData
            {
                Email = currentUser.EmailAddress,
                FirstName = currentUser.GivenName,
                LastName = currentUser.Surname,
                Groups = groupNames
            };
        }

        private static string GetUserGroups(UserPrincipal currentUser)
        {
            var rawGroupName = new List<string>();
            var groups = currentUser.GetGroups();
            var iterGroup = groups.GetEnumerator();
            using (iterGroup)
            {
                while (iterGroup.MoveNext())
                {
                    try
                    {
                        Principal p = iterGroup.Current;
                        rawGroupName.Add(p.Name);
                    }
                    catch (NoMatchingPrincipalException pex)
                    {
                    }
                }
            }
            var groupNames = rawGroupName.Aggregate((current, next) => current + "; " + next);
            
            return groupNames;
        }
    }
}