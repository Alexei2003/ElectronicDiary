using ElectronicDiary.Web.DTO.Responses.Educations;

namespace ElectronicDiary.Web.DTO.Responses.Users
{
    public record StudentParentResponse : BaseResponse
    {
        public BaseUserResponse SchoolStudent { get; init; } = new();
        public BaseUserResponse ParentType { get; init; } = new();
        public TypeResponse Type { get; init; } = new();
    }
}
