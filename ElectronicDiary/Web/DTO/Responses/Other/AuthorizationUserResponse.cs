namespace ElectronicDiary.Web.DTO.Responses.Other
{
    public record AuthorizationUserResponse
    {
        public long? Id { get; init; } = -1;

        public string? Role { get; set; } = null;

        public long UserId { get; init; } = -1;
    }
}
