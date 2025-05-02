namespace ElectronicDiary.Web.DTO.Responses.Other
{
    public record TypeResponse : BaseResponse
    {
        public string? Name { get; init; } = null;

        public TypeResponse() { }

        public TypeResponse(long id, string? name)
        {
            Id = id;
            Name = name ?? "ошибка";
        }
    }
}
