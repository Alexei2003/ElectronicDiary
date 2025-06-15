using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Web.Api.Educations
{
    public class GroupController : IController
    {
        public Task<string?> GetAll(long id)
        {
            string url = $"/findGroupsByClassId?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> GetById(long id)
        {
            throw new NotImplementedException();
        }
        public Task<string?> Edit(object request)
        {
            const string url = "/changeGroup";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }

        public Task<string?> Add(object request)
        {
            const string url = "/addGroup";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }

        public Task<string?> AddImage(long id, FileResult image)
        {
            throw new NotImplementedException();
        }

        public Task<string?> Delete(long id)
        {
            string url = $"/deleteGroupById?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }

        // Не интерфейсные методы
        public static Task<string?> GetGroupMembersByTeacherAssignment(long id)
        {
            string url = $"/findGroupMemberByTeacherAssignmentId?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
    }
}
