using System.Text;

namespace ElectronicDiary.Web.Api.Users
{
    public class AdministratorController : Controller
    {
        public override async Task<string?> GetAll(long id)
        {
            string url = $"/getAdministrators?schoolId={id}";
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public override async Task<string?> GetById(long id)
        {
            string url = $"/findAdministratorById?id={id}";
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public override async Task<string?> Add(string json)
        {
            const string url = "/addAdministrator";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, content);
        }

        public override async Task<string?> Delete(long id)
        {
            string url = $"/deleteAdministrator?id={id}";
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }


        // Стороние
        public async Task<string?> GetSchoolByAdministratorId(long id)
        {
            string url = $"/findSchoolByAdministratorId?id={id}";
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
    }
}
