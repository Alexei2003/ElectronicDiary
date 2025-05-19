using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Web.DTO.Responses.Educations
{
    public record SheduleLessonCustomResponse : BaseResponse
    {
        public List<SheduleLessonElemCustomResponse> Lessons { get; set; } = [];

        public long DayNumber { get; set; } = -1;
    }
}
