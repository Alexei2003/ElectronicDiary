namespace ElectronicDiary.Web.DTO.Responses.Users
{
    public record StudentParentResponse : BaseResponse
    {
        public BaseUserResponse? SchoolStudent { get; init; } = null;
        public BaseUserResponse? Parent { get; init; } = null;
        public TypeResponse? ParentType { get; init; } = null;
    }
}
