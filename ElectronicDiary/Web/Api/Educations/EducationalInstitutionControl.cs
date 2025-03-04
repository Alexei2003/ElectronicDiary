using ElectronicDiary.Web.DTO.Requests;
using System.Net.Http.Json;

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

        public static Task<HttpClientCustom.Response> AddEducationalInstitution(EducationalInstitutionRequest dto)
        {
            const string url = "/addEducationalInstitution";
            var content = JsonContent.Create(dto);
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, content);
        }

        public static Task<HttpClientCustom.Response> DeleteEducationalInstitution(long id)
        {
            string url = $"/deleteEducationalInstitution?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }
    }
}
