namespace ElectronicDiary.Web.DTO.Responses.Users
{
    public record StudentParentResponse : BaseResponse
    {
        public UserResponse? SchoolStudent { get; init; } = null;
        public UserResponse? Parent { get; init; } = null;
        public TypeResponse? ParentType { get; init; } = null;
    }
}
