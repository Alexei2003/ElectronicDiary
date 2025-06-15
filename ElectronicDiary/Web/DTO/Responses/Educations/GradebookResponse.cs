using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Web.DTO.Responses.Educations
{
    public record GradebookResponse : BaseResponse
    {
        public List<GradebookDayResponse>? GradebookDayResponseDTOS { get; set; } = null;
        public List<GradebookAttendanceResponse>? GradebookAttendanceResponseDTOS { get; set; } = null;
        public List<GradebookScoreResponse>? GradebookScoreResponseDTOS { get; set; } = null;
    }
}
