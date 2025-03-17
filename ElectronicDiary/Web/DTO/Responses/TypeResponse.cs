namespace ElectronicDiary.Web.DTO.Responses
{
    public record TypeResponse : BaseResponse
    {
        public string? Name { get; init; } = null;
    }
}
