using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Web.Api.Educations
{
    public static class AddressСontroller
    {
        public static Task<string?> GetRegions()
        {
            const string url = "/getRegions";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public static Task<string?> GetSettlements(long regionId)
        {
            string url = $"/getSettlements?region={regionId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
    }
}
