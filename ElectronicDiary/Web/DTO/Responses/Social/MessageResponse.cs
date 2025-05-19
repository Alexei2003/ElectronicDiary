
using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Web.DTO.Responses.Social
{
    public record MessageResponse : BaseResponse
    {
        public long GetterUserId { get; init; } = -1;
        public string? GetterFirstName { get; init; } = null;
        public string? GetterLastName { get; init; } = null;
        public string? GetterPatronymic { get; init; } = null;

        public long SenderUserId { get; init; } = -1;
        public string? SenderFirstName { get; init; } = null;
        public string? SenderLastName { get; init; } = null;
        public string? SenderPatronymic { get; init; } = null;

        public string? Message { get; init; } = null;
        public DateTime? DateTime { get; init; } = null;
    }
}
