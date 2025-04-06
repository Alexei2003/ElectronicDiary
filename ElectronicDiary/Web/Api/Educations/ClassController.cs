using System.Text;

namespace ElectronicDiary.Web.Api.Educations
{
    public class ClassController : IController
    {
        public Task<string?> GetAll(long schoolId)
        {
            string url = $"/getClasses?schoolId={schoolId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> GetById(long id)
        {
            string url = $"/findClassByTeacherId?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> Add(string json)
        {
            const string url = "/addClass";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, content);
        }

        public Task<string?> Delete(long id)
        {
            string url = $"/deleteClass?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }

        // Не интерфейсные методы
        public Task<string?> GetClassByTeacher(long id)
        {
            string url = $"/findClassByTeacherId?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> GetTeacherByClass(long id)
        {
            string url = $"/getTeacherOfClass?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
    }
}
