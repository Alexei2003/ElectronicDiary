using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Web.DTO.Responses.Educations
{
    public record QuarterScoreResponse : BaseResponse
    {
        public long QuarterNumber { get; set; } = -1;
        public long SchoolStudentId { get; set; } = -1;
        public SchoolSubjectResponse? SchoolSubject { get; set; } = null;
        public long Score { get; set; } = -1;
    }
}
