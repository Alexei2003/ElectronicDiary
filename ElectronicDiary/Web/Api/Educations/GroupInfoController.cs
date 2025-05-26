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
            throw new NotImplementedException();
        }

        public Task<string?> Add(object request)
        {
            const string url = "/addGroupMember";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }

        public Task<string?> Edit(object request)
        {
            throw new NotImplementedException();
        }

        public Task<string?> Delete(long id)
        {
            string url = $"/deleteGroupMember?studentId={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }

        public Task<string?> AddImage(long id, FileResult image)
        {
            throw new NotImplementedException();
        }

        // Не интерфейсные методы
        public Task<string?> Add(long studentId, long groupId)
        {
            string url = $"/addGroupMember?studentId={studentId}&groupId={groupId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url);
        }

        public Task<string?> Delete(long studentId, long groupId)
        {
            string url = $"/deleteGroupMember?studentId={studentId}&groupid={groupId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }
    }
}
