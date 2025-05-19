using ElectronicDiary.Web.DTO.Responses.Other;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Web.DTO.Responses.Social
{
    public record ChatResponse : BaseResponse
    {
        public List<MessageResponse> Messages { get; set; } = [];
        public UserResponse User { get; set; } = new();
    }
}
