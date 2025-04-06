using System.Net.Http.Headers;
using System.Text;

using ElectronicDiary.Web.Api.Other;

using Microsoft.AspNetCore.Http;

namespace ElectronicDiary.Web.Api.Users
{
    public class SchoolStudentController : IController
    {
        public Task<string?> GetAll(long schoolId)
        {
            string url = $"/getSchoolStudents?schoolId={schoolId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> GetById(long id)
        {
            string url = $"/findSchoolStudentById?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> Add(string json)
        {
            const string url = "/addSchoolStudent";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, content);
        }
        public Task<string?> Edit(string json)
        {
            const string url = "/changeSchoolStudent";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, content);
        }
        public Task<string?> Delete(long id)
        {
            string url = $"/deleteSchoolStudent?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }
        public Task<string?> AddImage(long id, FileResult  image)
        {
            string url = $"/addImageSchoolStudent?id={id}";

            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(image.OpenReadAsync().Result);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);
            content.Add(fileContent, "image", image.FileName);

            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, content);
        }

        // Не интерфейсные методы
        //Получить школу по id ученика
        public static Task<string?> GetSchoolBySchoolStudentId(long id)
        {
            string url = $"/findSchoolBySchoolStudentId?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        //Получить учеников по id класса
        public static Task<string?> GetStudentsOfClass(long objectId)
        {
            string url = $"/getStudentsOfClass?ObjectId={objectId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
    }
}
