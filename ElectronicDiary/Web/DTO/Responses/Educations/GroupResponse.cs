using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Web.DTO.Responses.Educations
{
    public record GroupResponse : BaseResponse
    {
        public ClassResponse? ClassRoom { get; set; } = null;
        public string GroupName { get; set; } = string.Empty;
    }
}
