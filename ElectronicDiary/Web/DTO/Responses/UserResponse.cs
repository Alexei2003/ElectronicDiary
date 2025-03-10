namespace ElectronicDiary.Web.DTO.Responses
{
    public record UserResponse
    {
        public long Id { get; init; }
        public string Login { get; init; } = "";
        public byte[] Hash { get; init; } = Array.Empty<byte>();
        public byte[] Salt { get; init; } = Array.Empty<byte>();
        public UserTypeResponse UserType { get; init; } = new();
    }
}
