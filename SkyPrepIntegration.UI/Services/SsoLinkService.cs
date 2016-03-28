using System.Web;
using SkyPrepIntegration.UI.Models;

namespace SkyPrepIntegration.UI.Services
{
    using System;
    using System.Configuration;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Newtonsoft.Json;

    public class SsoLinkService
    {
        private readonly Apisetting _apiSetting;

        public SsoLinkService(Apisetting apiSetting)
        {
            _apiSetting = apiSetting;
        }

        public string GenerateSsoLink(UserData userData)
        {
            if (string.IsNullOrEmpty(userData.Email))
            {
                return string.Empty;
            }
            var client = new HttpClient
            {
                BaseAddress = _apiSetting.Uri
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var urlParameters= BuildRequestUrl(userData);

            var response = client.GetAsync(urlParameters).Result;

            if (response.IsSuccessStatusCode)
            {
                var dataObjects = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);
                return dataObjects.url;
            }

            return string.Empty;
        }

        private string BuildRequestUrl(UserData userData)
           {
            var formatReplacer = "?user_email={0}&autocreate={1}&user_first_name={2}&user_last_name={3}&key_type={4}&api_key={6}&acct_key={7}";
            if (_apiSetting.IncludeGroups)
            {
                formatReplacer = "?user_email={0}&autocreate={1}&user_first_name={2}&user_last_name={3}&key_type={4}&user_groups={5}&api_key={6}&acct_key={7}";
            }
            Console.WriteLine(formatReplacer);
            var urlParameters = string.Format(formatReplacer,
                userData.Email, _apiSetting.AutoCreate, userData.FirstName, userData.LastName, _apiSetting.KeyType, userData.Groups, _apiSetting.ApiKey, _apiSetting.AccountKey);

            return HttpUtility.HtmlEncode(urlParameters);
        }
    }

    public class Apisetting
    {
        public bool IncludeGroups { get; set; }
        public string KeyType { get; set; }
        public bool AutoCreate { get; set; }
        public string ApiKey { get; set; }
        public Uri Uri { get; set; }
        public string UserGroups { get; set; }
        public string AccountKey { get; set; }

        public Apisetting(string apiKey, Uri uri, string keyType, bool autoCreate, string userGroups, string accountKey, bool includeGroups)
        {
            KeyType = keyType;
            AutoCreate = autoCreate;
            ApiKey = apiKey;
            Uri = uri;
            UserGroups = userGroups;
            AccountKey = accountKey;
            IncludeGroups = includeGroups;

        }
      
        public static Apisetting Get()
        {
            var uri = ConfigurationManager.AppSettings["uri"].ToSafeString();
            var apiKey = ConfigurationManager.AppSettings["api_key"].ToSafeString();
            var accountKey = ConfigurationManager.AppSettings["acct_key"].ToSafeString();
            var keyType = ConfigurationManager.AppSettings["key_type"].ToSafeString();
            var autoCreate = ConfigurationManager.AppSettings["autocreate"].ToSafeBool();
            var includeGroups = ConfigurationManager.AppSettings["include_groups"].ToSafeBool();
            var userGroups = ConfigurationManager.AppSettings["user_groups"].ToSafeString();

            return new Apisetting(apiKey, new Uri(uri), keyType, autoCreate, userGroups, accountKey, includeGroups);
        }
    }

    public static class HelperExtensions
    {
        public static bool ToSafeBool(this string str)
        {
            bool result;

            if (bool.TryParse(str, out result))
            {
                return result;
            }

            return true;
        }

        public static string ToSafeString(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            return str;
        }
    }

    public class SsoResponse
    {
        //[DeserializeAs(Name = "email")]
        //public string Email { get; set; }

        //[DeserializeAs(Name = "login_key")]
        //public string LoginKey { get; set; }

        //[DeserializeAs(Name = "url")]
        //public string Url { get; set; }

    }
}