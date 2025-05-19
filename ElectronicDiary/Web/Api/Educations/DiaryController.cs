using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Web.Api.Educations
{
    public class DiaryController
    {
        public static Task<string?> GetAll(long schoolStudentId, long quarterId)
        {
            string url = $"/findDiaryInfoBySchoolStudentIdAndQuarter?id={schoolStudentId}&quarterNumber={quarterId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
    }
}
