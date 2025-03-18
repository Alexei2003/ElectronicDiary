namespace ElectronicDiary.Web.DTO.Requests.Users
{
    public class ParentRequest : UserRequest
    {
        public long? ParentType { get; set; } = null;
        public long? SchoolStudentId { get; set; } = null;
    }
}
