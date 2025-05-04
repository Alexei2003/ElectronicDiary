using ElectronicDiary.Web.DTO.Requests.Educations.Other;

namespace ElectronicDiary.Web.DTO.Requests.Educations
{
    public class GroupRequest : BaseRequest
    {
        public long ClassRoom { get; set; } = -1;
        public string GroupName { get; set; } = string.Empty;
    }
}
