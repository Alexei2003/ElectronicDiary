using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Web.DTO.Responses.Educations
{
    public record DiaryResponse : BaseResponse
    {
        public SchoolSubjectResponse? SchoolSubject { get; init; } = null;
        public DateTime? DateTime { get; init; } = null;
        public bool Attendance { get; init; } = false;
        public long Score { get; init; } = -1;
    }
}
