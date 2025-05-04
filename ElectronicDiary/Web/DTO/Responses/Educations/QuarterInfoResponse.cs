using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Web.DTO.Responses.Educations
{
    public record QuarterInfoResponse : BaseResponse
    {
        public long QuarterNumber { get; set; } = -1;
        public DateTime? DateStartTime { get; set; } = null;
        public DateTime? DateEndTime { get; set; } = null;
    }
}
