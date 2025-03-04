namespace ElectronicDiary.Web.DTO.Responses
{
    public record RegionResponse
    {
        public int Id { get; init; }
        public string Name { get; init; } = "";
    }
}
