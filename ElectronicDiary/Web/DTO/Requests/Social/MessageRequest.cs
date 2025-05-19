using ElectronicDiary.Web.DTO.Requests.Educations.Other;

namespace ElectronicDiary.Web.DTO.Requests.Social
{
    public class MessageRequest : BaseRequest
    {
        public long GetterUserId { get; set; } = -1;
        public long SenderUserId { get; set; } = -1;
        public string? Message { get; set; } = null;
        public DateTime? DateTime { get; set; } = null;
    }
}
