using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Web.DTO.Responses.Educations
{
    public record SchoolSubjectResponse : BaseResponse
    {
        public string Name { get; set; } = string.Empty;
    }
}
