namespace SkyPrepIntegration.UI.Services
{
    using System;
    using System.Configuration;
    using System.Net.Http;
    using System.Web;
    using System.Net.Http.Headers;
    using Newtonsoft.Json;
    using System.Collections.Specialized;
    using System.Diagnostics;



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

            NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["user_email"] = emailAddress;
            queryString["autocreate"] = _apiSetting.AutoCreate;
            queryString["key_type"] = _apiSetting.KeyType;
            queryString["api_key"] = _apiSetting.ApiKey;
            queryString["acct_key"] = _apiSetting.Acctkey;
            queryString["first_name"] = fname;
            queryString["last_name"] = lname;
            queryString["autocreate_user_groups"] = _apiSetting.AutoCreateUserGroups;
            queryString["user_groups"] = userGroups;
            Debug.WriteLine(queryString.ToString());
            return "?" + queryString.ToString();
            
        }
    }

    public class Apisetting
    {
        public string KeyType { get; set; }
        public string AutoCreate { get; set; }
        public string AutoCreateUserGroups { get; set; }
        public string ApiKey { get; set; }
        public Uri Uri { get; set; }
        public string Acctkey { get; set; }

        public Apisetting(string apiKey, Uri uri, string keyType, string autoCreate, string acctkey, string autoCreateUserGroups)
        {
            KeyType = keyType;
            AutoCreate = autoCreate;
            ApiKey = apiKey;
            Acctkey = acctkey;
            AutoCreateUserGroups = autoCreateUserGroups;
        }
      
        public static Apisetting Get()
        {
            var acct_key = ConfigurationManager.AppSettings["acct_key"].ToSafeString();
            var apiKey = ConfigurationManager.AppSettings["api_key"].ToSafeString();
            var keyType = ConfigurationManager.AppSettings["key_type"].ToSafeString();
            var autoCreate = ConfigurationManager.AppSettings["autocreate"].ToSafeString();
            var autoCreateUserGroups = ConfigurationManager.AppSettings["autocreate_user_groups"].ToSafeString();
            var uri = new Uri("https://api.skyprep.io/admin/api/get_login_key");
            return new Apisetting(apiKey, uri, keyType, autoCreate, acct_key, autoCreateUserGroups);
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

    }
}