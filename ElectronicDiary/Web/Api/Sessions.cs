using System.Net;

namespace ElectronicDiary.Web.Api
{
    public static class Sessions
    {
        public static Task<HttpClientCustom.Response> LogIn(string login, string password)
        {
            var url = $"/login?login={WebUtility.UrlEncode(login)}&password={WebUtility.UrlEncode(password)}";

            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url);
        }

        public static Task<HttpClientCustom.Response> logOut()
        {
            const string url = $"/logout";

            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
    }
}
