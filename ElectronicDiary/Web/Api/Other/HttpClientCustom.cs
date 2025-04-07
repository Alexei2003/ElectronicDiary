using System.Net;

using ElectronicDiary.Pages;
using ElectronicDiary.Pages.Others;

using System.Runtime.Caching;

namespace ElectronicDiary.Web.Api.Other
{
    public static class HttpClientCustom
    {
        private static readonly HttpClientHandler _handler = new()
        {
            UseCookies = true,
            CookieContainer = new CookieContainer()
        };

        private static readonly HttpClient _httpClient = new(_handler)
        {
            BaseAddress = new Uri(WebConstants.BASE_URL),
            Timeout = TimeSpan.FromSeconds(30)
        };

        // Добавляем кеш в память
        private static readonly MemoryCache _cache = MemoryCache.Default;
        private static readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(5);

        public enum HttpTypes
        {
            GET, POST, PUT, DELETE
        }

        public static async Task<string?> CheckResponse(HttpTypes httpTypes, string url, HttpContent? content = null)
        {
            HttpResponseMessage response;
            try
            {
                response = httpTypes switch
                {
                    HttpTypes.GET => await _httpClient.GetAsync(url),
                    HttpTypes.POST => await _httpClient.PostAsync(url, content),
                    HttpTypes.PUT => await _httpClient.PutAsync(url, content),
                    HttpTypes.DELETE => await _httpClient.DeleteAsync(url),
                    _ => throw new ArgumentOutOfRangeException(nameof(httpTypes), httpTypes, null)
                };

                // Статус код 200-299
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                var message = "Что-то пошло не так. Если ошибка повторяется, сообщите в поддержку";
                if (ex is HttpRequestException httpEx)
                {
                    if (httpEx.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            var window = Application.Current?.Windows[0];
                            if (window != null) window.Page = new ThemedNavigationPage(new LogPage());
                        });
                    }

                    message = httpEx.StatusCode switch
                    {
                        HttpStatusCode.Unauthorized => "Войдите в систему для доступа",
                        HttpStatusCode.Forbidden => "Доступ к этому разделу запрещён",
                        HttpStatusCode.NotFound => "Информация не найдена",
                        HttpStatusCode.InternalServerError => "Проблема на сервере. Попробуйте позже",
                        HttpStatusCode.ServiceUnavailable => "Сервис временно не работает",
                        HttpStatusCode.GatewayTimeout or HttpStatusCode.RequestTimeout => "Слишком долгий ответ. Проверьте интернет и повторите",
                        _ => "Ошибка связи с сервером"
                    };
                }

                Application.Current.Dispatcher.Dispatch(() =>
                {
                    var page = Application.Current?.Windows[0].Page;
                    page?.DisplayAlert("Ошибка", message, "OK");
                });
                return null;
            }

            return await response.Content.ReadAsStringAsync();
        }

        //public static string SerializeCookies()
        //{
        //    var cookiesList = _handler.CookieContainer.GetAllCookies();
        //    return JsonSerializer.Serialize(cookiesList);
        //}

        //public static void DeserializeCookies(string json)
        //{
        //    var cookiesList = JsonSerializer.Deserialize<CookieCollection>(json);
        //    _handler.CookieContainer.Add(cookiesList);
        //}
    }
}
