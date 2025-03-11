using System.Text;
using System.Threading.Tasks;

namespace ElectronicDiary.Web.Api.Education
{
    public class ClassController : IController
    {

        public async Task<string?> GetAll(long schoolId)
        {
            string url = $"/getClasses?schoolId={schoolId}";
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public async Task<string?> GetById(long id)
        {
            string url = $"/findClassByTeacherId?id={id}";
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public async Task<string?> Add(string json)
        {
            const string url = "/addClass";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, content);
        }

        public async Task<string?> Delete(long id)
        {
            string url = $"/deleteClass?id={id}";
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }
    }
}
