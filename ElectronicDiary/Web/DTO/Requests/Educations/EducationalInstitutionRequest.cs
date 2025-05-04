using ElectronicDiary.Web.DTO.Requests.Educations.Other;

namespace ElectronicDiary.Web.DTO.Requests.Educations
{
    public class EducationalInstitutionRequest : BaseRequest
    {
        public string? Name { get; set; } = null;
        public string? Address { get; set; } = null;
        public string? Email { get; set; } = null;
        public string? PhoneNumber { get; set; } = null;
        public long RegionId { get; set; } = -1;
        public long SettlementId { get; set; } = -1;
    }
}
