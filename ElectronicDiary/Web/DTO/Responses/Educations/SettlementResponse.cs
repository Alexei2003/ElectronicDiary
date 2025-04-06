using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Web.DTO.Responses.Educations
{
    public record SettlementResponse : BaseResponse
    {
        public string? Name { get; init; } = null;
        public RegionResponse? Region { get; init; } = null;
    }
}
