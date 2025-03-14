using ElectronicDiary.Web.DTO.Responses.Educations;

namespace ElectronicDiary.Web.DTO.Responses.Users
{
    public record BaseUserResponse : BaseResponse
    {
        public UserResponse User { get; init; } = new();
        public EducationalInstitutionResponse EducationalInstitution { get; init; } = new();
        public string FirstName { get; init; } = "";
        public string LastName { get; init; } = "";
        public string? Patronymic { get; init; }
        public string? PathImage { get; init; }
        public string? Email { get; init; }
        public string? PhoneNumber { get; init; }
    }
}
