namespace ElectronicDiary.Web.Api
{
    public static class Admin
    {
        public static Task<HttpClientCustom.Response> GetSchools()
        {
            const string url = "/getSchools";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public static Task<HttpClientCustom.Response> GetSchoolsById(long id)
        {
            string url = $"/getSchoolsById?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        //public static Task<HttpClientCustom.Response> AddEducationalInstitution(EducationalInstitutionDTO dto)
        //{
        //    const string url = "/addEducationalInstitution";
        //    return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, dto);
        //}

        public static Task<HttpClientCustom.Response> DeleteEducationalInstitution(long id)
        {
            string url = $"/deleteEducationalInstitution?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }

        public static Task<HttpClientCustom.Response> GetSchoolStudents(long schoolId)
        {
            string url = $"/getSchoolStudents?schoolId={schoolId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        //public static Task<HttpClientCustom.Response> AddSchoolStudent(SchoolStudentDTO dto)
        //{
        //    const string url = "/addSchoolStudent";
        //    return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, dto);
        //}

        public static Task<HttpClientCustom.Response> GetRegions()
        {
            const string url = "/getRegions";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public static Task<HttpClientCustom.Response> GetSettlements(long regionId)
        {
            string url = $"/getSettlements?region={regionId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public static Task<HttpClientCustom.Response> GetAdministrators(long schoolId)
        {
            string url = $"/getAdministrators?schoolId={schoolId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        //public static Task<HttpClientCustom.Response> AddAdministrator(AdministratorDTO dto)
        //{
        //    const string url = "/addAdministrator";
        //    return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, dto);
        //}

        public static Task<HttpClientCustom.Response> DeleteAdministrator(long id)
        {
            string url = $"/deleteAdministrator?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }

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

        public static Task<HttpClientCustom.Response> GetClasses(long schoolId)
        {
            string url = $"/getClasses?schoolId={schoolId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        //public static Task<HttpClientCustom.Response> AddClass(ClassDTO dto)
        //{
        //    const string url = "/addClass";
        //    return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, dto);
        //}

        public static Task<HttpClientCustom.Response> DeleteClass(long id)
        {
            string url = $"/deleteClass?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }

        public static Task<HttpClientCustom.Response> GetStudentParents(long studentId)
        {
            string url = $"/getStudentParents?SchoolStudentId={studentId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        //public static Task<HttpClientCustom.Response> AddParent(ParentDTO dto)
        //{
        //    const string url = "/addParents";
        //    return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, dto);
        //}

        public static Task<HttpClientCustom.Response> DeleteParent(long id)
        {
            string url = $"/deleteParent?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }
    }
}
