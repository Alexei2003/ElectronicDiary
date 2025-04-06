using System.Net.Http.Headers;
using System.Text;

using ElectronicDiary.Web.Api.Other;

using Microsoft.AspNetCore.Http;

namespace ElectronicDiary.Web.Api.Users
{
    public class ParentController : IController
    {
        public Task<string?> GetAll(long schoolId)
        {
            string url = $"/getParentsByEducationId?id={schoolId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> GetById(long id)
        {
            string url = $"/findParentById?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> Add(string json)
        {
            const string url = "/addNewParent";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, content);
        }
        public Task<string?> Edit(string json)
        {
            const string url = "/changeParent";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, content);
        }
        public Task<string?> Delete(long id)
        {
            string url = $"/deleteParent?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }

        public Task<string?> AddImage(long id, FileResult  image)
        {
            string url = $"/addImageParent?id={id}";

            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(image.OpenReadAsync().Result);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(image.ContentType);
            content.Add(fileContent, "image", image.FileName);

            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, content);
        }

        // Не интерфейсные методы
        // Получить родителей исключив родителей по id ребёнка
        public static Task<string?> GetParentsWithoutSchoolStudent(long schoolStudentId)
        {
            string url = $"/getNewParents?id={schoolStudentId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        // Добавить уже существующего родителя ребёнку
        public static Task<string?> AddParent(string json)
        {
            const string url = "/addParent";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, content);
        }

        // Удалить связь StudentParent
        public static Task<string?> DeleteStudentParent(long id)
        {
            string url = $"/deleteStudentParent?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }

        // Получить тип родителя
        public static Task<string?> GetParentType()
        {
            const string url = "/getParentType";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        // Получить StudentParent по id ученика
        public static Task<string?> GetStudentParents(long schoolStudentId)
        {
            string url = $"/getStudentParents?id={schoolStudentId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        // Получить StudentParent по id родителя
        public static Task<string?> GetParentStudents(long parentId)
        {
            string url = $"/getStudentParentsByParentTd?id={parentId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

    }
}
