using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Web.Api.Users
{
    public class ParentWithStudentsController : StudentParentController
    {
        public override Task<string?> GetAll(long id)
        {
            string url = $"/getStudentParentsByParentTd?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
    }
}
