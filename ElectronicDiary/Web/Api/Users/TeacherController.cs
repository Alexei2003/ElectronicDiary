using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Web.Api.Users
{
    public class TeacherController : IController
    {
        public Task<string?> GetAll(long id)
        {
            string url = $"/getTeachers?schoolId={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> GetById(long id)
        {
            string url = $"/findTeacherById?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> Add(object request)
        {
            const string url = "/addTeacher";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }

        public Task<string?> Edit(object request)
        {
            const string url = "/changeTeacher";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }
        public Task<string?> Delete(long id)
        {
            string url = $"/deleteTeacher?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }

        public Task<string?> AddImage(long id, FileResult image)
        {
            string url = $"/addImageTeacher?id={id}";

            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, image: image);
        }

        // Не интерфейсные методы
        public static Task<string?> GetSchoolByTeacherId(long id)
        {
            string url = $"/findSchoolByTeacherId?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public static Task<string?> GetTeachersToClass(long id)
        {
            string url = $"/getTeachersToClass?schoolId={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
    }
}
