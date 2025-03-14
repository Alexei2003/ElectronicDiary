using System.Text;

namespace ElectronicDiary.Web.Api.Users
{
    public class ParentController : IController
    {
        public async Task<string?> GetAll(long schoolStudentId)
        {
            string url = $"/getParents?schoolId={schoolStudentId}";
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public async Task<string?> GetById(long id)
        {
            string url = $"/findParentById?id={id}";
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public async Task<string?> Add(string json)
        {
            const string url = "/addNewParent";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, content);
        }

        public async Task<string?> Delete(long id)
        {
            string url = $"/deleteParent?id={id}";
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }

        // Не интерфейсные методы

        public async Task<string?> GetNewParents(long schoolStudentId)
        {
            string url = $"/getNewParents?id={schoolStudentId}";
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public static async Task<string?> AddParent(string json)
        {
            const string url = "/addParent";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, content);
        }

        public static async Task<string?> DeleteStudentParent(long id)
        {
            string url = $"/deleteStudentParent?id={id}";
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }

        public async Task<string?> GetParentType()
        {
            const string url = "/getParentType";
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
    }
}
