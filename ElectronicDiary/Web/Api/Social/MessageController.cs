using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Web.Api.Social
{
    public class MessageController : IController
    {
        public Task<string?> GetAll(long id)
        {
            return Task.Run(() => { return (string?)"empty method"; });
        }

        public Task<string?> GetById(long id)
        {
            string url = $"/getMessageById?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> Add(object request)
        {
            const string url = "/addMessage";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }

        public Task<string?> Edit(object request)
        {
            const string url = "/changeMessage";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }

        public Task<string?> Delete(long id)
        {
            string url = $"/deleteMessageById?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }

        public Task<string?> AddImage(long id, FileResult image)
        {
            return Task.Run(() => { return (string?)"empty method"; });
        }

        // Не интерфейсные методы
        public static Task<string?> FindLastMessagesToUser(long id)
        {
            string url = $"/findLatestMessageByGetterUserId?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public static Task<string?> GetAllByGetterId(long id)
        {
            string url = $"/findMessageByGetterUserId?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public static Task<string?> GetAllBySenderId(long id)
        {
            string url = $"/findMessageBySenderUserId?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
    }
}
