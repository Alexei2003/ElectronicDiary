namespace ElectronicDiary.Web.Api.Educations
{
    public static class AddressControl
    {
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
    }
}
