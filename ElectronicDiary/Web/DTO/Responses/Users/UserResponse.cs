using ElectronicDiary.Web.DTO.Responses.Educations;

namespace ElectronicDiary.Web.DTO.Responses.Users
{
    public record UserResponse : BaseResponse
    {
        public EducationalInstitutionResponse? EducationalInstitution { get; init; } = null;
        public string? FirstName { get; init; } = null;
        public string? LastName { get; init; } = null;
        public string? Patronymic { get; init; }
        public string? PathImage { get; init; }
        public string? Email { get; init; }
        public string? PhoneNumber { get; init; }
    }
}
