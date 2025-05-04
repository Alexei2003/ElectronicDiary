using ElectronicDiary.Web.DTO.Responses.Educations;

namespace ElectronicDiary.Web.DTO.Responses.Users
{
    public record SchoolStudentResponse : UserResponse
    {
        public ClassResponse? ClassRoom { get; init; } = null;
    }
}
