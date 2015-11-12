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

        public string GenerateSsoLink(string emailAddress)
        {
            if (string.IsNullOrEmpty(emailAddress))
            {
                return string.Empty;
            }
            var client = new HttpClient
            {
                BaseAddress = _apiSetting.Uri
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var urlParameters= BuildRequestUrl(emailAddress);

            var response = client.GetAsync(urlParameters).Result;

            if (response.IsSuccessStatusCode)
            {
                var dataObjects = JsonConvert.DeserializeObject<dynamic>(response.Content.ReadAsStringAsync().Result);
                return dataObjects.url;
            }

            return string.Empty;
        }

        private string BuildRequestUrl(string emailAddress)
        {
            var urlParameters = string.Format("?user_email={0}&autocreate={1}&key_type={2}&user_groups={3}&api_key={4}",
                emailAddress, _apiSetting.AutoCreate, _apiSetting.KeyType, _apiSetting.UserGroups, _apiSetting.ApiKey);

            return urlParameters;
        }
    }

    public class Apisetting
    {
        public string KeyType { get; set; }
        public bool AutoCreate { get; set; }
        public string ApiKey { get; set; }
        public Uri Uri { get; set; }
        public string UserGroups { get; set; }

        public Apisetting(string apiKey, Uri uri, string keyType, bool autoCreate, string userGroups)
        {
            KeyType = keyType;
            AutoCreate = autoCreate;
            ApiKey = apiKey;
            Uri = uri;
            UserGroups = userGroups;
        }
      
        public static Apisetting Get()
        {
            var uri = ConfigurationManager.AppSettings["uri"].ToSafeString();
            var apiKey = ConfigurationManager.AppSettings["api_key"].ToSafeString();
            var keyType = ConfigurationManager.AppSettings["key_type"].ToSafeString();
            var autoCreate = ConfigurationManager.AppSettings["autocreate"].ToSafeBool();
            var userGroups = ConfigurationManager.AppSettings["user_groups"].ToSafeString();

            return new Apisetting(apiKey, new Uri(uri), keyType, autoCreate, userGroups);
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