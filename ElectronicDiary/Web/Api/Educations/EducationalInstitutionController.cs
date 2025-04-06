using System.Net.Http.Headers;
using System.Text;

using ElectronicDiary.Web.Api.Other;

using Microsoft.AspNetCore.Http;

namespace ElectronicDiary.Web.Api.Educations
{
    public class EducationalInstitutionСontroller : IController
    {
        public Task<string?> GetAll(long schoolId)
        {
            const string url = "/getSchools";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> GetById(long id)
        {
            string url = $"/getSchoolsById?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
        public Task<string?> Edit(string json)
        {
            const string url = "/changeEducationalInstitution";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, content);
        }

        public Task<string?> Add(string json)
        {
            const string url = "/addEducationalInstitution";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, content);
        }

        public Task<string?> AddImage(long id, IFormFile image)
        {
            string url = $"/addImageEducational?id={id}";

            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(image.OpenReadStream());
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);
            content.Add(fileContent, "image", image.FileName);

            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, content);
        }

        public Task<string?> Delete(long id)
        {
            string url = $"/deleteEducationalInstitution?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }
    }
}
