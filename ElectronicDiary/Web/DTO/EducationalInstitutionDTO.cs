using System.ComponentModel.DataAnnotations;

namespace ElectronicDiary.Web.DTO
{
    public record EducationalInstitutionDTO
    {
        public int Id { get; init; }
        public string Name { get; init; } = default!;
        public string Address { get; init; } = default!;
        public string? PathImage { get; init; }
        public string? Email { get; init; }
        public string? PhoneNumber { get; init; }
        public InstitutionTypeDTO EducationalInstitutionType { get; init; } = default!;
        public SettlementDTO Settlement { get; init; } = default!;
    }
}
