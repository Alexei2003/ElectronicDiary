using System.Text;
using System.Threading.Tasks;

namespace ElectronicDiary.Web.Api.Users
{
    public class TeacherController : IController
    {
        public async Task<string?> GetAll(long schoolId)
        {
            string url = $"/getTeachers?schoolId={schoolId}";
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public async Task<string?> GetById(long id)
        {
            string url = $"/findTeacherById?id={id}";
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public async Task<string?> Add(string json)
        {
            const string url = "/addTeacher";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, content);
        }

        public async Task<string?> Delete(long id)
        {
            string url = $"/deleteTeacher?id={id}";
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }

        // Не интерфейсные методы
        public async Task<string?> GetSchoolByTeacherId(long id)
        {
            string url = $"/findSchoolByTeacherId?id={id}";
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public async Task<string?> GetTeachersToClass(long schoolId)
        {
            string url = $"/getTeachersToClass?schoolId={schoolId}";
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
    }
}
