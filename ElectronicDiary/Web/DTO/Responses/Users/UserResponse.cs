namespace ElectronicDiary.Web.DTO.Responses.Users
{
    public record UserResponse : BaseResponse
    {
        public string Login { get; init; } = "";
        public byte[] Hash { get; init; } = [];
        public byte[] Salt { get; init; } = [];
        public UserTypeResponse UserType { get; init; } = new();
    }
}
