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

        public string GenerateSsoLink(string emailAddress, string fName, string lName, string userGroups)
        {
            var baseUri = "https://api.skyprep.io/admin/api/get_login_key";
            if (string.IsNullOrEmpty(emailAddress))
            {
                return string.Empty;
            }
            var client = new HttpClient
            {
                BaseAddress = new Uri(baseUri)
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var urlParameters= BuildRequestUrl(emailAddress, fName, lName, userGroups);

            var response = client.GetAsync(urlParameters).Result;

            if (response.IsSuccessStatusCode)
            {
                var dataObjects = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);
                return dataObjects.url;
            }

            return string.Empty;
        }

        private string BuildRequestUrl(string emailAddress, string fname, string lname, string userGroups)
        {
            var urlParameters = string.Format("?user_email={0}&autocreate={1}&key_type={2}&user_groups={3}&api_key={4}&acct_key={5}&first_name={6}&last_name={7}",
                emailAddress, _apiSetting.AutoCreate, _apiSetting.KeyType, userGroups, _apiSetting.ApiKey, _apiSetting.Acctkey, fname, lname);

            return urlParameters;
        }
    }

    public class Apisetting
    {
        public string KeyType { get; set; }
        public string AutoCreate { get; set; }
        public string ApiKey { get; set; }
        public Uri Uri { get; set; }
        public string Acctkey { get; set; }

        public Apisetting(string apiKey, Uri uri, string keyType, string autoCreate, string acctkey)
        {
            KeyType = keyType;
            AutoCreate = autoCreate;
            ApiKey = apiKey;
            Acctkey = acctkey;
        }
      
        public static Apisetting Get()
        {
            var acct_key = ConfigurationManager.AppSettings["acct_key"].ToSafeString();
            var apiKey = ConfigurationManager.AppSettings["api_key"].ToSafeString();
            var keyType = ConfigurationManager.AppSettings["key_type"].ToSafeString();
            var autoCreate = ConfigurationManager.AppSettings["autocreate"].ToSafeString();

            var uri = new Uri("https://api.skyprep.io/admin/api/get_login_key");

            return new Apisetting(apiKey, uri, keyType, autoCreate, acct_key);
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