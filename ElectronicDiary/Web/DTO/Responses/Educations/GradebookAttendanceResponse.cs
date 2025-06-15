using ElectronicDiary.Web.DTO.Responses.Other;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Web.DTO.Responses.Educations
{
    public record GradebookAttendanceResponse : BaseResponse
    {
        public GradebookDayResponse? GradebookDay { get; set; } = null;

        public SchoolStudentResponse? SchoolStudent { get; set; } = null;
    }
}
