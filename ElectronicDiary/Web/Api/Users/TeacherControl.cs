namespace ElectronicDiary.Web.Api.Users
{
    public static class TeacherСontroller
    {
        public static Task<string?> GetTeachers(long schoolId)
        {
            string url = $"/getTeachers?schoolId={schoolId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        //public static Task<string?> AddTeacher(TeacherDTO dto)
        //{
        //    const string url = "/addTeacher";
        //    return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, dto);
        //}

        public static Task<string?> DeleteTeacher(long id)
        {
            string url = $"/deleteTeacher?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }
    }
}
