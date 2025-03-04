using ElectronicDiary.Web.DTO;
using System.Text.Json;
using System.Text;

namespace ElectronicDiary.Web.Api.Educations
{
    public static class EducationalInstitutionControl
    {
        public static Task<HttpClientCustom.Response> GetSchools()
        {
            const string url = "/getSchools";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public static Task<HttpClientCustom.Response> GetSchoolsById(long id)
        {
            string url = $"/getSchoolsById?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public static Task<HttpClientCustom.Response> AddEducationalInstitution(EducationalInstitutionDTO dto)
        {
            const string url = "/addEducationalInstitution";
            using var content = new StringContent(
                JsonSerializer.Serialize(dto),
                Encoding.UTF8,
                "application/json");
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, content);
        }

        public static Task<HttpClientCustom.Response> DeleteEducationalInstitution(long id)
        {
            string url = $"/deleteEducationalInstitution?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }
    }
}
