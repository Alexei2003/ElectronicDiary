namespace ElectronicDiary.Web.DTO.Requests
{
    public class EducationalInstitutionRequest
    {
        public string Name { get; set; } = "";
        public string Address { get; set; } = "";
        public string? Email { get; set; } = "";
        public string? PhoneNumber { get; set; } = "";
        public long RegionId { get; set; } = -1;
        public long SettlementId { get; set; } = -1;
    }
}
