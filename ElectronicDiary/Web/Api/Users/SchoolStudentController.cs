using System.Text;

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

        public Task<string?> Delete(long id)
        {
            string url = $"/deleteSchoolStudent?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
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
