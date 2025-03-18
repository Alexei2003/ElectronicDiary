using System.Text;

namespace ElectronicDiary.Web.Api.Users
{
    public class AdministratorController : IController
    {
        public Task<string?> GetAll(long schoolId)
        {
            string url = $"/getAdministrators?schoolId={schoolId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }


        public Task<string?> GetById(long id)
        {
            string url = $"/findAdministratorById?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> Add(string json)
        {
            const string url = "/addAdministrator";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, content);
        }

        public Task<string?> Delete(long id)
        {
            string url = $"/deleteAdministrator?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }


        // Не интерфейсные методы
        public static Task<string?> GetSchoolByAdministratorId(long id)
        {
            string url = $"/findSchoolByAdministratorId?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
    }
}
