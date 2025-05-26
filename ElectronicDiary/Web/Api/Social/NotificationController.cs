using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Web.Api.Social
{
    public class NotificationController : IController
    {
        public Task<string?> GetAll(long id)
        {
            string url = $"/findNotificationByUserId?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> GetById(long id)
        {
            throw new NotImplementedException();
        }
        public Task<string?> Edit(object request)
        {
            throw new NotImplementedException();
        }

        public Task<string?> Add(object request)
        {
            throw new NotImplementedException();
        }

        public Task<string?> AddImage(long id, FileResult image)
        {
            throw new NotImplementedException();
        }

        public Task<string?> Delete(long id)
        {
            string url = $"/deleteNotificationById?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }
    }
}
