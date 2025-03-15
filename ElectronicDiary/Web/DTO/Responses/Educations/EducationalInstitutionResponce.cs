namespace ElectronicDiary.Web.DTO.Responses.Educations
{
    public record EducationalInstitutionResponse : BaseResponse
    {
        public string Name { get; init; } = "";
        public string Address { get; init; } = "";
        public string? PathImage { get; init; } = "";
        public string? Email { get; init; } = "";
        public string? PhoneNumber { get; init; } = "";
        public TypeResponse EducationalInstitutionType { get; init; } = new();
        public SettlementResponse Settlement { get; init; } = new();
    }
}
