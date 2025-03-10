namespace ElectronicDiary.Web.Api.Users
{
    public static class ParentСontroller
    {
        public static Task<string?> GetStudentParents(long studentId)
        {
            string url = $"/getStudentParents?SchoolStudentId={studentId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        //public static Task<string?> AddParent(ParentDTO dto)
        //{
        //    const string url = "/addParents";
        //    return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url, dto);
        //}

        public static Task<string?> DeleteParent(long id)
        {
            string url = $"/deleteParent?id={id}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }
    }
}
