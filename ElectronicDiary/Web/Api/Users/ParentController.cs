using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Web.Api.Users
{
    public class ParentController : IController
    {
        public Task<string?> GetAll(long id)
        {
            string url = $"/getParentsByEducationId?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> GetById(long id)
        {
            string url = $"/findParentById?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> Add(object request)
        {
            const string url = "/addNewParent";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }
        public Task<string?> Edit(object request)
        {
            const string url = "/changeParent";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }
        public Task<string?> Delete(long id)
        {
            string url = $"/deleteParent?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }

        public Task<string?> AddImage(long id, FileResult image)
        {
            string url = $"/addImageParent?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, image: image);
        }
    }
}
