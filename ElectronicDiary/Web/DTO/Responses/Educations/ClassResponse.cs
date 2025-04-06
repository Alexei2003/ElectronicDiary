using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Web.DTO.Responses.Educations
{
    public record ClassResponse : BaseResponse
    {
        public string? Name { get; init; } = null;
        public UserResponse? Teacher { get; init; } = null;
    }
}
