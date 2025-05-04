using ElectronicDiary.Web.DTO.Requests.Educations.Other;

namespace ElectronicDiary.Web.DTO.Requests.Users
{
    public class StudentParentRequest : BaseRequest
    {
        public long SchoolStudentId { get; set; } = -1;
        public long ParentId { get; set; } = -1;
        public long ParentTypeId { get; set; } = -1;
    }
}
