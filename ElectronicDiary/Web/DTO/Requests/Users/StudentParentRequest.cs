namespace ElectronicDiary.Web.DTO.Requests.Users
{
    public class StudentParentRequest
    {
        public long? SchoolStudentId { get; set; } = null;
        public long? ParentId { get; set; } = null;
        public long? ParentTypeId { get; set; } = null;
    }
}
