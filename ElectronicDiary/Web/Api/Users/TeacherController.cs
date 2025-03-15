using System.Text;

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

        public Task<string?> Delete(long id)
        {
            string url = $"/deleteTeacher?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
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
