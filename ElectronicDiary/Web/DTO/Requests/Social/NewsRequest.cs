using ElectronicDiary.Web.DTO.Requests.Other;

namespace ElectronicDiary.Web.DTO.Requests.Social
{
    public class NewsRequest : BaseRequest
    {
        public long EducationalInstitutionId { get; set; } = -1;
        public long OwnerUserId { get; set; } = -1;
        public string? Title { get; set; } = null;
        public string? Content { get; set; } = null;
        public DateTime? DateTime { get; set; } = null;
    }
}
