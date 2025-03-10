namespace ElectronicDiary.Web.DTO.Responses
{
    public record UserTypeResponse
    {
        public long Id { get; init; }
        public string Name { get; init; } = "";
    }
}
