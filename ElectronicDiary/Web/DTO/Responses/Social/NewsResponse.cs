using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Web.DTO.Responses.Social
{
    public record NewsResponse : BaseResponse
    {
        public long UserId { get; init; } = -1;
        public long EducationId { get; init; } = -1;
        public string? FirstName { get; init; } = null;
        public string? LastName { get; init; } = null;
        public string? Patronymic { get; init; } = null;
        public string? Title { get; init; } = null;
        public string? Content { get; init; } = null;
        public DateTime? DateTime { get; init; } = null;
    }
}
