using System.Net.Http.Headers;
using System.Text;

using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Web.Api.Users
{
    public class TeacherController : IController
    {
        public Task<string?> GetAll(long schoolId)
        {
            string url = $"/getTeachers?schoolId={schoolId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> GetById(long id)
        {
            string url = $"/findTeacherById?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> Add(string json)
        {
            const string url = "/addTeacher";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, content);
        }

        public Task<string?> Edit(string json)
        {
            const string url = "/changeTeacher";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, content);
        }
        public Task<string?> Delete(long id)
        {
            string url = $"/deleteTeacher?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }

        public Task<string?> AddImage(long id, FileResult image)
        {
            string url = $"/addImageTeacher?id={id}";

            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(image.OpenReadAsync().Result);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);
            content.Add(fileContent, "image", image.FileName);

            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, content);
        }

        // Не интерфейсные методы
        public static Task<string?> GetSchoolByTeacherId(long id)
        {
            string url = $"/findSchoolByTeacherId?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public static Task<string?> GetTeachersToClass(long schoolId)
        {
            string url = $"/getTeachersToClass?schoolId={schoolId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
    }
}
