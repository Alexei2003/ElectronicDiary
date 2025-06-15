using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Web.Api.Educations
{
    public class GradebookController
    {
        public static Task<string?> FindBySchoolStudent(long schoolStudentId, long quarterId)
        {
            string url = $"/findDiaryInfoBySchoolStudentIdAndQuarter?id={schoolStudentId}&quarterNumber={quarterId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public static Task<string?> FindByTeacherAssignment(long teacherAssignmentId, long quarterId)
        {
            string url = $"/findGradebookDayByScheduleLessonTeacherAssignmentId?id={teacherAssignmentId}&quarterId={quarterId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public static Task<string?> FindSchoolSubjectsBySchoolStudent(long schoolStudentId)
        {
            string url = $"/findSchoolSubjectsBySchoolStudentId?id={schoolStudentId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.GET, url);
        }

        public static Task<string?> UpdateGradebookDay(long gradebookDayId, string topic, string homework)
        {
            string url = $"/updateGradebookDay?id={gradebookDayId}&topic={Uri.EscapeDataString(topic)}&homework={Uri.EscapeDataString(homework)}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url);
        }

        public static Task<string?> UpdateGradebookScore(long gradebookDayId, long schoolStudentId, long score)
        {
            string url = $"/updateGradebookScore?gradebookDayId={gradebookDayId}&schoolStudentId={schoolStudentId}&score={score}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url);
        }

        public static Task<string?> UpdateGradebookAttendance(long gradebookDayId, long schoolStudentId)
        {
            string url = $"/updateGradebookAttendance?gradebookDayId={gradebookDayId}&schoolStudentId={schoolStudentId}";
            return HttpClientCustom.CheckResponse(HttpClientCustom.HttpTypes.POST, url);
        }
    }
}
