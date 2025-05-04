namespace ElectronicDiary.Web.DTO.Requests.Users
{
    public class SchoolStudentRequest : UserRequest
    {
        public long ClassRoomId { get; set; } = -1;
    }
}
