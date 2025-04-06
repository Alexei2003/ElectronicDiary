using ElectronicDiary.Web.DTO.Requests.Other;

namespace ElectronicDiary.Web.DTO.Requests.Users
{
    public class UserRequest : BaseRequest
    {
        public string? FirstName { get; set; } = null;
        public string? LastName { get; set; } = null;
        public string? Patronymic { get; set; }
        public string? Login { get; set; } = null;
        public string? Password { get; set; } = null;
        public string? Email { get; set; } = null;
        public string? PhoneNumber { get; set; } = null;
        public long UniversityId { get; set; } = -1;
    }
}
