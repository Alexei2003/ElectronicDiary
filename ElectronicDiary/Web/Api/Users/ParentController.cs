using System.Text;

namespace ElectronicDiary.Web.Api.Users
{
    public class ParentController : IController
    {
        public async Task<string?> GetAll(long schoolId)
        {
            string url = $"/getParentsByEducationId?id={schoolId}";
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
        // Получить родителей исключив родителей по id ребёнка
        public async Task<string?> GetNewParents(long schoolStudentId)
        {
            string url = $"/getNewParents?id={schoolStudentId}";
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        // Добавить уже существующего родителя ребёнку
        public static async Task<string?> AddParent(string json)
        {
            const string url = "/addParent";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, content);
        }

        // Удалить связь StudentParent
        public static async Task<string?> DeleteStudentParent(long id)
        {
            string url = $"/deleteStudentParent?id={id}";
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }

        // Получить тип родителя
        public static async Task<string?> GetParentType()
        {
            const string url = "/getParentType";
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        // Получить учеников по id родителя
        public async Task<string?> GetStudentsOfParent(long parentId)
        {
            string url = $"/getStudentsOfParent?ObjectId={parentId}";
            return await HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
    }
}
