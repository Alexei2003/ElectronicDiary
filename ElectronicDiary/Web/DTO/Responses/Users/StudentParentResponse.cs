namespace ElectronicDiary.Web.DTO.Responses.Users
{
    public record StudentParentResponse : BaseResponse
    {
        public BaseUserResponse SchoolStudent { get; init; } = new();
        public BaseUserResponse Parent { get; init; } = new();
        public TypeResponse ParentType { get; init; } = new();
    }
}
