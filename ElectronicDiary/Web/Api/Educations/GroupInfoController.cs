using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Web.Api.Educations
{
    public class GroupInfoController : IController
    {
        public Task<string?> GetAll(long id)
        {
            string url = $"/findGroupMemberByGroupId?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> GetById(long id)
        {
            return Task.Run(() => { return "empty method"; });
        }

        public Task<string?> Add(object request)
        {
            const string url = "/addGroupMember";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }

        public Task<string?> Edit(object request)
        {
            return Task.Run(() => { return "empty method"; });
        }

        public Task<string?> Delete(long id)
        {
            string url = $"/deleteGroupMember?studentId={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }

        public Task<string?> AddImage(long id, FileResult image)
        {
            return Task.Run(() => { return "empty method"; });
        }

        // Не интерфейсные методы
        public Task<string?> Add(long studentId, long groupId)
        {
            string url = $"/addGroupMember?studentId={studentId}&groupid={groupId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url);
        }

        public Task<string?> Delete(long studentId, long groupId)
        {
            string url = $"/deleteGroupMember?studentId={studentId}&groupid={groupId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }
    }
}
