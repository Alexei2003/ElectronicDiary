namespace ElectronicDiary.Web.DTO.Responses.Educations
{
    public record SettlementResponse : BaseResponse
    {
        public string Name { get; init; } = "";
        public RegionResponse Region { get; init; } = new();
    }
}
