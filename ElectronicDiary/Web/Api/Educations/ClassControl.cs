namespace ElectronicDiary.Web.Api.Educations
{
    public static class ClassControl
    {
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
    }
}
