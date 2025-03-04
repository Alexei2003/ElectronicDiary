namespace ElectronicDiary.Web.DTO.Responses
{
    public record InstitutionTypeResponse
    {
        public int Id { get; init; }
        public string Name { get; init; } = "";
    }
}
