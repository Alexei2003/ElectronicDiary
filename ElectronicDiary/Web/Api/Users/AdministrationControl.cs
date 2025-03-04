namespace ElectronicDiary.Web.Api.Users
{
    public static class AdministrationControl
    {
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
    }
}
