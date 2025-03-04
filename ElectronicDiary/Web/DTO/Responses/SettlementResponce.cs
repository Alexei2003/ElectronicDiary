namespace ElectronicDiary.Web.DTO.Responses
{
    public record SettlementResponse
    {
        public int Id { get; init; }
        public string Name { get; init; } = "";
        public RegionResponse Region { get; init; } = new();
    }
}
