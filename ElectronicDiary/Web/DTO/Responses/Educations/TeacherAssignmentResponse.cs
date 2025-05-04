using ElectronicDiary.Web.DTO.Responses.Other;
using ElectronicDiary.Web.DTO.Responses.Users;

namespace ElectronicDiary.Web.DTO.Responses.Educations
{
    public record TeacherAssignmentResponse : BaseResponse
    {
        public UserResponse? Teacher { get; set; } = null;
        public SchoolSubjectResponse? SchoolSubject { get; set; } = null;
        public GroupResponse? Group { get; set; } = null;
    }
}
