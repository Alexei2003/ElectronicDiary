namespace ElectronicDiary.Web.Api.Users
{
    public static class SchoolStudentСontroller
    {
        public static Task<string?> GetSchoolStudents(long schoolId)
        {
            string url = $"/getSchoolStudents?schoolId={schoolId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        //public static Task<string?> AddSchoolStudent(SchoolStudentDTO dto)
        //{
        //    const string url = "/addSchoolStudent";
        //    return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, dto);
        //}
    }
}
