﻿using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Web.Api.Educations
{
    public class SheduleLessonController : IController
    {
        public Task<string?> GetAll(long id)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetById(long id)
        {
            throw new NotImplementedException();
        }

        public Task<string?> Add(object request)
        {
            throw new NotImplementedException();
        }

        public Task<string?> Edit(object request)
        {
            throw new NotImplementedException();
        }

        public Task<string?> Delete(long id)
        {
            throw new NotImplementedException();
        }

        public Task<string?> AddImage(long id, FileResult image)
        {
            throw new NotImplementedException();
        }

        // Не интерфейсные методы
        public Task<string?> GetAll(long id, long quarter)
        {
            string url = $"/findLessonsBySchoolStudentId?id={id}&quarter={quarter}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
    }
}
