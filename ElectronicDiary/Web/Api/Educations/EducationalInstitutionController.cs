using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Web.Api.Educations
{
    public class EducationalInstitutionСontroller : IController
    {
        public Task<string?> GetAll(long schoolId)
        {
            const string url = "/getSchools";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> GetById(long id)
        {
            string url = $"/getSchoolsById?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
        public Task<string?> Edit(object request)
        {
            const string url = "/changeEducationalInstitution";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }

        public Task<string?> Add(object request)
        {
            const string url = "/addEducationalInstitution";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }

        public Task<string?> AddImage(long id, FileResult image)
        {
            string url = $"/addImageEducational?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, image: image);
        }

        public Task<string?> Delete(long id)
        {
            string url = $"/deleteEducationalInstitution?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }
    }
}
