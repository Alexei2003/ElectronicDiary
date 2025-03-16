namespace ElectronicDiary.Web.DTO.Requests.Users
{
    public class StudentParentRequest
    {
        public long SchoolStudentId { get; set; }
        public long ParentId { get; set; }
        public long ParentTypeId { get; set; }
    }
}
