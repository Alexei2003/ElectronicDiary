﻿using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Web.Api.Users
{
    public class AdministratorController : IController
    {
        public Task<string?> GetAll(long id)
        {
            string url = $"/getAdministrators?schoolId={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }


        public Task<string?> GetById(long id)
        {
            string url = $"/findAdministratorById?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> Add(object request)
        {
            const string url = "/addAdministrator";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }

        public Task<string?> Edit(object request)
        {
            const string url = "/changeAdministrator";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }

        public Task<string?> Delete(long id)
        {
            string url = $"/deleteAdministrator?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }

        public Task<string?> AddImage(long id, FileResult image)
        {
            string url = $"/addImageAdministrator?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, image: image);
        }

        // Не интерфейсные методы
        public static Task<string?> GetSchoolByAdministratorId(long id)
        {
            string url = $"/findSchoolByAdministratorId?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
    }
}
