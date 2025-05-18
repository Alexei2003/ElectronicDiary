using ElectronicDiary.Web.DTO.Responses.Educations;
using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Web.DTO.Responses.Users
{
    public record UserResponse : BaseResponse
    {
        public EducationalInstitutionResponse? EducationalInstitution { get; init; } = null;
        public string? FirstName { get; init; } = null;
        public string? LastName { get; init; } = null;
        public string? Patronymic { get; init; } = null;
        public string? PathImage { get; init; } = null;
        public string? Email { get; init; } = null;
        public string? PhoneNumber { get; init; } = null;

        public long UserId { get; init; } = -1;
    }
}
