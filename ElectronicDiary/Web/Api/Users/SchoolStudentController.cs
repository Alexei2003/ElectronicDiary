using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Web.Api.Users
{
    public class SchoolStudentController : IController
    {
        public Task<string?> GetAll(long schoolId)
        {
            string url = $"/getSchoolStudents?schoolId={schoolId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> GetById(long id)
        {
            string url = $"/findSchoolStudentById?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public Task<string?> Add(object request)
        {
            const string url = "/addSchoolStudent";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }
        public Task<string?> Edit(object request)
        {
            const string url = "/changeSchoolStudent";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, request);
        }
        public Task<string?> Delete(long id)
        {
            string url = $"/deleteSchoolStudent?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }
        public Task<string?> AddImage(long id, FileResult image)
        {
            string url = $"/addImageSchoolStudent?id={id}";

            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, image: image);
        }

        // Не интерфейсные методы
        //Получить школу по id ученика
        public static Task<string?> GetSchoolBySchoolStudentId(long id)
        {
            string url = $"/findSchoolBySchoolStudentId?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        //Получить учеников по id класса
        public static Task<string?> GetStudentsOfClass(long objectId)
        {
            string url = $"/getStudentsOfClass?ObjectId={objectId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
    }
}
