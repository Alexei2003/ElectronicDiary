namespace ElectronicDiary.Web.Api.Users
{
    public static class TeacherControl
    {
        public static Task<HttpClientCustom.Response> GetTeachers(long schoolId)
        {
            string url = $"/getTeachers?schoolId={schoolId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        //public static Task<HttpClientCustom.Response> AddTeacher(TeacherDTO dto)
        //{
        //    const string url = "/addTeacher";
        //    return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, dto);
        //}

        public static Task<HttpClientCustom.Response> DeleteTeacher(long id)
        {
            string url = $"/deleteTeacher?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }
    }
}
