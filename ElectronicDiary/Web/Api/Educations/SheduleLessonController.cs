using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Web.Api.Educations
{
    public class SheduleLessonController : IController
    {
        public Task<string?> GetAll(long id)
        {
            return Task.Run(() => { return (string?)"empty method"; });
        }

        public Task<string?> GetById(long id)
        {
            return Task.Run(() => { return (string?)"empty method"; });
        }

        public Task<string?> Add(object request)
        {
            return Task.Run(() => { return (string?)"empty method"; });
        }

        public Task<string?> Edit(object request)
        {
            return Task.Run(() => { return (string?)"empty method"; });
        }

        public Task<string?> Delete(long id)
        {
            return Task.Run(() => { return (string?)"empty method"; });
        }

        public Task<string?> AddImage(long id, FileResult image)
        {
            return Task.Run(() => { return (string?)"empty method"; });
        }

        // Не интерфейсные методы
        public Task<string?> GetAll(long id, long quarter)
        {
            string url = $"/findLessonsBySchoolStudentId?id={id}&quarter={quarter}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
    }
}
