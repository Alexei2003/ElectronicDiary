using ElectronicDiary.Web.DTO.Responses.Other;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Web.DTO.Responses.Educations
{
    public record GroupMemberResponse : BaseResponse
    {
        public GroupResponse? Group { get; set; } = null;
        public SchoolStudentResponse? SchoolStudent { get; set; } = null;
    }
}
