namespace ElectronicDiary.Web.DTO.Responses.Other
{
    public record AuthorizationUserResponse : BaseResponse
    {
        public string? Role { get; set; } = null;
    }
}
