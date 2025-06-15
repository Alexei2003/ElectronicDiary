using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Web.DTO.Responses.Educations
{
    public record GradebookDayResponse : BaseResponse
    {
        public SheduleLessonResponse? ScheduleLesson { get; set; } = null;
        public DateTime? DateTime { get; set; } = null;
        public string? Topic { get; set; } = string.Empty;
        public string? Homework { get; set; } = string.Empty;
    }
}
