using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Web.Api.Educations
{
    public class QuarterScoreController
    {
        public static Task<string?> FindByTeacherAssignmentAndQuarter(long teacherAssignmentId, long quarterId)
        {
            string url = $"/findQuarterScoreByTeacherAssignmentIdAndQuarterInfoId?teacherAssignmentId={teacherAssignmentId}&quarterId={quarterId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public static Task<string?> FindByTeacherAssignment(long teacherAssignmentId)
        {
            string url = $"/findQuarterScoreByTeacherAssignmentId?teacherAssignmentId={teacherAssignmentId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public static Task<string?> FindByStudent(long schoolStudentId)
        {
            string url = $"/findQuarterScoreBySchoolStudentId?schoolStudentId={schoolStudentId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public static Task<string?> AddScore(long schoolStudentId, long schoolSubjectId, long quarterId, long score)
        {
            string url = $"/addQuarterScore?schoolStudentId={schoolStudentId}&schoolSubjectId={schoolSubjectId}&quarterId={quarterId}&score={score}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url);
        }

        public static Task<string?> UpdateScore(long quarterScoreId, long score)
        {
            string url = $"/updateQuarterScore?quarterScoreId={quarterScoreId}&score={score}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url);
        }

        public static Task<string?> GetQuarterScoresForJournal(long teacherAssignmentId, long quarterId)
        {
            string url = $"/findQuarterScoreByTeacherAssignmentIdAndQuarterInfoId?teacherAssignmentId={teacherAssignmentId}&quarterId={quarterId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }
    }
}
