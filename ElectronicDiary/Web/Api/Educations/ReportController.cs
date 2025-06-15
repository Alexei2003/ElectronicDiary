using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Web.Api.Educations
{
    public class ReportController
    {
        public static Task<string?> FindReportsByEducationalId(long educationalId)
        {
            string url = $"/findReportsByEducationalId?educationalId={educationalId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public static Task<HttpResponseMessage> DownloadReport(long reportId)
        {
            string url = $"/downloadReport?reportId={reportId}";
            return HttpClientCustom.SendRequest(HttpClientCustom.HttpTypes.GET, url);
        }

        public static Task<string?> CreateReport(long educationalInstitutionId)
        {
            string url = $"/createReport?educationalInstitutionId={educationalInstitutionId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url);
        }

        public static Task<string?> DeleteReportById(long reportId)
        {
            string url = $"/deleteReportById?id={reportId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.DELETE, url);
        }
    }
}
