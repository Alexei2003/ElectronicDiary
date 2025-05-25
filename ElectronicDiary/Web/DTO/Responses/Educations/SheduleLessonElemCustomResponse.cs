namespace ElectronicDiary.Web.DTO.Responses.Educations
{
    public record SheduleLessonElemCustomResponse
    {
        public TeacherAssignmentResponse? TeacherAssignment { get; set; } = null;
        public long Number { get; set; } = -1;
        public string? Room { get; set; } = null;

        public List<DiaryResponse>? DiaryList { get; set; } = null;
    }
}
