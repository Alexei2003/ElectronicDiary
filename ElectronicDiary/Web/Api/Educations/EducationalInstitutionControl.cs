﻿using System.Text;

namespace ElectronicDiary.Web.Api.Educations
{
    public class EducationalInstitutionСontroller : IController
    {
        public async Task<string?> GetAll(long schoolId)
        {
            const string url = "/getSchools";
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public async Task<string?> GetById(long id)
        {
            string url = $"/getSchoolsById?id={id}";
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public async Task<string?> Add(string json)
        {
            const string url = "/addEducationalInstitution";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, content);
        }

        public async Task<string?> Delete(long id)
        {
            string url = $"/deleteEducationalInstitution?id={id}";
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }
    }
}
