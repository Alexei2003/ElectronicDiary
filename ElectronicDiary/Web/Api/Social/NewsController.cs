using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Web.Api.Social
{
    public class NewsController : IController
    {
        public Task<string?> GetAll(long educationId)
        {
            string url = $"/findNewsByEducationId?id={educationId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> GetById(long id)
        {
            string url = $"/findNewsById?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> Add(object request)
        {
            const string url = "/addNews";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }

        public Task<string?> Edit(object request)
        {
            const string url = "/changeNews";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }

        public Task<string?> Delete(long id)
        {
            string url = $"/deleteNewsById?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }

        public Task<string?> AddImage(long id, FileResult image)
        {
            return Task.Run(() => "empty method");
        }

        // Не интерфейсные методы
        public Task<string?> DeleteComment(long commentId)
        {
            string url = $"/deleteNewsCommentById?id={commentId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }
    }
}
