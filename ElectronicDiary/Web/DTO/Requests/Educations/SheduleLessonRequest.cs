using ElectronicDiary.Web.DTO.Requests.Educations.Other;

namespace ElectronicDiary.Web.DTO.Requests.Educations
{
    public class SheduleLessonRequest : BaseRequest
    {
        public long Quarter { get; set; } = -1;
        public long DayNumber { get; set; } = -1;
        public long LessonNumber { get; set; } = -1;
        public long GroupId { get; set; } = -1;
        public long SubjectId { get; set; } = -1;
        public long TeacherId { get; set; } = -1;
    }
}
