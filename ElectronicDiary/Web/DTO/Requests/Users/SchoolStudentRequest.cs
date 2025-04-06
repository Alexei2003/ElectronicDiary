namespace ElectronicDiary.Web.DTO.Requests.Users
{
    public class SchoolStudentRequest : UserRequest
    {
        public long ClassId { get; set; } = -1;
    }
}
