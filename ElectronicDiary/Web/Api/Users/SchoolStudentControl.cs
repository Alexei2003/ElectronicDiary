using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicDiary.Web.Api.Users
{
    public static class SchoolStudentControl
    {
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
    }
}
