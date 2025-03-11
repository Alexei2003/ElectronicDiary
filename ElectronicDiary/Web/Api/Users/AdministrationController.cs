using System.Text;

namespace ElectronicDiary.Web.Api.Users
{
    public class AdministratorController : IController
    {
        public async Task<string?> GetAll(long schoolId)
        {
            string url = $"/getAdministrators?schoolId={schoolId}";
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public async Task<string?> GetById(long id)
        {
            string url = $"/findAdministratorById?id={id}";
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public async Task<string?> Add(string json)
        {
            const string url = "/addAdministrator";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, content);
        }

        public async Task<string?> Delete(long id)
        {
            string url = $"/deleteAdministrator?id={id}";
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }


        // Не интерфейсные методы
        public async Task<string?> GetSchoolByAdministratorId(long id)
        {
            string url = $"/findSchoolByAdministratorId?id={id}";
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
    }
}
