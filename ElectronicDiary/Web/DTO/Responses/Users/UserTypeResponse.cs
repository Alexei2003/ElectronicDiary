namespace ElectronicDiary.Web.DTO.Responses.Users
{
    public record UserTypeResponse : BaseResponse
    {
        public string Name { get; init; } = "";
    }
}
