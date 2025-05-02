using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Web.Api.Users
{
    public class StudentWithParentsController : StudentParentController
    {
        public override Task<string?> GetAll(long id)
        {
            string url = $"/getStudentParents?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        // Получить родителей исключив родителей по id ребёнка
        public override Task<string?> GetById(long id)
        {
            string url = $"/getNewParents?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
    }
}
