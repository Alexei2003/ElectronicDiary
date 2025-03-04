using System.ComponentModel.DataAnnotations;

namespace ElectronicDiary.Web.DTO
{
    public record EducationalInstitutionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string? PathImage { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public InstitutionTypeDTO EducationalInstitutionType { get; set; } = default!;
        public SettlementDTO Settlement { get; set; } = default!;
    }
}
