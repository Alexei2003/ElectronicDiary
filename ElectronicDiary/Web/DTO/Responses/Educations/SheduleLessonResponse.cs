using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Web.DTO.Responses.Educations
{
    public record SheduleLessonResponse : BaseResponse
    {
        public GroupResponse? Group { get; set; } = null;
        public TeacherAssignmentResponse? TeacherAssignment { get; set; } = null;
        public QuarterInfoResponse? QuarterInfo { get; set; } = null;
        public long DayNumber { get; set; } = -1;
        public long LessonNumber { get; set; } = -1;
        public string? Room { get; set; } = null;

    }
}
