using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Web.DTO.Responses.Educations
{
    public record YearScoreResponse : BaseResponse
    {
        public long SchoolStudentId { get; set; } = -1;
        public SchoolSubjectResponse? SchoolSubject { get; set; } = null;
        public long Score { get; set; } = -1;
    }
}
