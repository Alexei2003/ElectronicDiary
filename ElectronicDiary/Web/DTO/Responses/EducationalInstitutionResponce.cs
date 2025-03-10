namespace ElectronicDiary.Web.DTO.Responses
{
    public record EducationalInstitutionResponse
    {
        public long Id { get; init; }
        public string Name { get; init; } = "";
        public string Address { get; init; } = "";
        public string? PathImage { get; init; } = "";
        public string? Email { get; init; } = "";
        public string? PhoneNumber { get; init; } = "";
        public InstitutionTypeResponse EducationalInstitutionType { get; init; } = new();
        public SettlementResponse Settlement { get; init; } = new();
    }
}
