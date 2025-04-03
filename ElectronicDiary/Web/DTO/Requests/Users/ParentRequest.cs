namespace ElectronicDiary.Web.DTO.Requests.Users
{
    public class ParentRequest : UserRequest
    {
        public long ParentType { get; set; } = -1;
        public long SchoolStudentId { get; set; } = -1;
    }
}
