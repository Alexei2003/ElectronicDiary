using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Web.Api.Educations
{
    public class ClassController : IController
    {
        public Task<string?> GetAll(long id)
        {
            string url = $"/getClasses?schoolId={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> GetById(long id)
        {
            string url = $"/findClassByTeacherId?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> Add(object request)
        {
            const string url = "/addClass";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }

        public Task<string?> Edit(object request)
        {
            const string url = "/changeClass";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }

        public Task<string?> Delete(long id)
        {
            string url = $"/deleteClass?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }

        public Task<string?> AddImage(long id, FileResult image)
        {
            return Task.Run(() => { return "empty method"; });
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
