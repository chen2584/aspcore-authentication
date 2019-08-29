using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using WebApiFacebookOAuth.Models;

namespace WebApiFacebookOAuth.Services
{
    public class FacebookOAuthService
    {
        readonly AppSetting _setting;
        readonly IHttpClientFactory _httpClientFactory;
        readonly UtilService _utilService;
        public FacebookOAuthService(AppSetting setting, IHttpClientFactory httpClientFactory, UtilService utilService)
        {
            _setting = setting;
            _httpClientFactory = httpClientFactory;
            _utilService = utilService;
        }

        private string GetRedirectUri()
        {
            var baseUri = _utilService.GetBaseUri();
            // var redirectUri = $"{baseUri}/api/Account/facebook/signin";
            var redirectUri = baseUri;
            return redirectUri;
        }

        public string GetAuthorizationUrl()
        {
            var appId = HttpUtility.UrlEncode(_setting.Authentication.Facebook.AppId);
            var rawRedirectUri = GetRedirectUri();
            var redirectUri = HttpUtility.UrlEncode(rawRedirectUri);
            var uri = $"https://www.facebook.com/v4.0/dialog/oauth?client_id={appId}&redirect_uri={redirectUri}&scope=email";
            return uri;
        }

        public async Task<string> GetAccessTokenAsync(string authorizationCode)
        {
            var appId = HttpUtility.UrlEncode(_setting.Authentication.Facebook.AppId);
            var appSecret = HttpUtility.UrlEncode(_setting.Authentication.Facebook.AppSecret);
            var rawRedirectUri = GetRedirectUri();
            var redirectUri = HttpUtility.UrlEncode(rawRedirectUri);
            var code = HttpUtility.UrlEncode(authorizationCode);
            var uri = $"https://graph.facebook.com/v4.0/oauth/access_token?client_id={appId}&redirect_uri={redirectUri}&client_secret={appSecret}&code={authorizationCode}";

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(uri);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var oAuthAccessToken = JsonConvert.DeserializeObject<FacebookOAuthAccessToken>(responseContent);
                return oAuthAccessToken.AccessToken;
            }
            else
            {
                throw new Exception(responseContent);
            }
        }

        public async Task<FacebookOAuthUserInfo> GetUserInfoAsync(string accessToken)
        {
            var uri = $"https://graph.facebook.com/v4.0/me?access_token={accessToken}&fields=id%2Cname%2Cfirst_name%2Clast_name%2Cemail&format=json";

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(uri);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var userInfo = JsonConvert.DeserializeObject<FacebookOAuthUserInfo>(responseContent);
                return userInfo;
            }
            else
            {
                throw new Exception(responseContent);
            }
        }
    }
}