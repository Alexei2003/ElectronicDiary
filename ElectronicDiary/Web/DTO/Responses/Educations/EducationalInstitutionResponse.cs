using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Web.DTO.Responses.Educations
{
    public record EducationalInstitutionResponse : BaseResponse
    {
        public string? Name { get; init; } = null;
        public string? Address { get; init; } = null;
        public string? PathImage { get; init; } = null;
        public string? Email { get; init; } = null;
        public string? PhoneNumber { get; init; } = null;
        public TypeResponse? EducationalInstitutionType { get; init; } = null;
        public SettlementResponse? Settlement { get; init; } = null;
    }
}
