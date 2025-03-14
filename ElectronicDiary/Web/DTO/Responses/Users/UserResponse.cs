namespace ElectronicDiary.Web.DTO.Responses.Users
{
    public record UserResponse : BaseResponse
    {
        public string Login { get; init; } = "";
        public byte[] Hash { get; init; } = Array.Empty<byte>();
        public byte[] Salt { get; init; } = Array.Empty<byte>();
        public UserTypeResponse UserType { get; init; } = new();
    }
}
