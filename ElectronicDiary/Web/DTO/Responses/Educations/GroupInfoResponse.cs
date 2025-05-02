using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Web.DTO.Responses.Educations
{
    public record GroupInfoResponse : BaseResponse
    {
        public GroupResponse? Group { get; set; } = null;
        public GroupMemberResponse[] GroupMembers { get; set; } = [];
    }
}
