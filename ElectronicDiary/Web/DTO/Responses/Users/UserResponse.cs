namespace ElectronicDiary.Web.DTO.Responses.Users
{
    public record UserResponse : BaseResponse
    {
        public string? Login { get; init; } = null;
        public byte[]? Hash { get; init; } = null;
        public byte[]? Salt { get; init; } = null;
        public TypeResponse? UserType { get; init; } = null;
    }
}
