using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Web.DTO.Responses.Educations
{
    public record RegionResponse : BaseResponse
    {
        public string? Name { get; init; } = null;
    }
}
