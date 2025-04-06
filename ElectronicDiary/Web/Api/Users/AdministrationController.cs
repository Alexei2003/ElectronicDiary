using System.Net.Http.Headers;
using System.Text;

using ElectronicDiary.Web.Api.Other;

using Microsoft.AspNetCore.Http;

namespace ElectronicDiary.Web.Api.Users
{
    public class AdministratorController : IController
    {
        public Task<string?> GetAll(long schoolId)
        {
            string url = $"/getAdministrators?schoolId={schoolId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }


        public Task<string?> GetById(long id)
        {
            string url = $"/findAdministratorById?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> Add(string json)
        {
            const string url = "/addAdministrator";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, content);
        }

        public Task<string?> Edit(string json)
        {
            const string url = "/changeAdministrator";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, content);
        }

        public Task<string?> Delete(long id)
        {
            string url = $"/deleteAdministrator?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }

        public Task<string?> AddImage(long id, FileResult  image)
        {
            string url = $"/addImageAdministrator?id={id}";

            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(image.OpenReadAsync().Result);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);
            content.Add(fileContent, "image", image.FileName);

            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, content);
        }

        // Не интерфейсные методы
        public static Task<string?> GetSchoolByAdministratorId(long id)
        {
            string url = $"/findSchoolByAdministratorId?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
    }
}
