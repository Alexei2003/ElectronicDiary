namespace ElectronicDiary.Web.DTO.Requests.Users
{
    public class ParentRequest : BaseUserRequest
    {
        public long? ParentType { get; set; } = null;
        public long? SchoolStudentId { get; set; } = null;
    }
}
