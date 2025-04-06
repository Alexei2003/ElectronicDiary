using ElectronicDiary.Web.DTO.Responses.Educations;

namespace ElectronicDiary.Web.DTO.Responses.Users
{
    public record SchoolStudentResponse : UserResponse
    {
        public ClassResponse? Class { get; init; } = null;
    }
}
