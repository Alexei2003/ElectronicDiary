using ElectronicDiary.Web.DTO.Requests.Other;

namespace ElectronicDiary.Web.DTO.Requests.Educations
{
    public class ClassRequest : BaseRequest
    {
        public string? Name { get; set; } = null;

        public long TeacherId { get; set; } = -1;
    }
}
