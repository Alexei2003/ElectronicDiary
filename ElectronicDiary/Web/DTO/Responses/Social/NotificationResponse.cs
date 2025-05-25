using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Web.DTO.Responses.Social
{
    public record NotificationResponse : BaseResponse
    {
        public long UserId { get; init; } = -1;
        public string Title { get; init; } = string.Empty;
        public string Content { get; init; } = string.Empty;
        public string Link { get; init; } = string.Empty;
        public DateTime? DateTime { get; init; } = null;
    }
}
