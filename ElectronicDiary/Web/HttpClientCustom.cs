﻿using ElectronicDiary.Pages;
using ElectronicDiary.Pages.Otherts;
using System.Net;

namespace ElectronicDiary.Web
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

        public enum HttpTypes
        {
            GET, POST, PUT, DELETE
        }

        public class Response
        {
            public bool Error { get; set; }
            public string Message { get; set; } = "";
        }

        public static async Task<Response> CheckResponse(HttpTypes httpTypes, string url, HttpContent? content = null)
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
                            var window = Application.Current?.Windows.FirstOrDefault();
                            if (window != null)
                            {
                                window.Page = new ThemedNavigationPage(new LogPage());
                            }
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

                return new Response()
                {
                    Error = true,
                    Message = message
                };
            }

            return new Response()
            {
                Error = false,
                Message = await response.Content.ReadAsStringAsync(),
            };
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
