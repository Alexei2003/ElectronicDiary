namespace ElectronicDiary.Web.Api.Users
{
    public static class AdministrationСontroller
    {
        public static Task<string?> GetAdministrators(long schoolId)
        {
            string url = $"/getAdministrators?schoolId={schoolId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        //public static Task<string?> AddAdministrator(AdministratorDTO dto)
        //{
        //    const string url = "/addAdministrator";
        //    return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, dto);
        //}

        public static Task<string?> DeleteAdministrator(long id)
        {
            string url = $"/deleteAdministrator?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }
    }
}
