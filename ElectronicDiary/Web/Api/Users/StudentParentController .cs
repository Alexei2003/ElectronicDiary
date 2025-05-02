using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Web.Api.Users
{
    public class StudentParentController : IController
    {
        public virtual Task<string?> GetAll(long id)
        {
            return Task.Run(() => { return "empty method"; });
        }

        public virtual Task<string?> GetById(long id)
        {
            return Task.Run(() => { return "empty method"; });
        }

        // Добавить уже существующего родителя ребёнку
        public virtual Task<string?> Add(object request)
        {
            const string url = "/addParent";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }
        public virtual Task<string?> Edit(object request)
        {
            return Task.Run(() => { return "empty method"; });
        }
        public virtual Task<string?> Delete(long id)
        {
            string url = $"/deleteStudentParent?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }

        public virtual Task<string?> AddImage(long id, FileResult image)
        {
            return Task.Run(() => { return "empty method"; });
        }

        // Не интерфейсные методы

        // Получить тип родителя
        public static Task<string?> GetParentType()
        {
            const string url = "/getParentType";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
    }
}
