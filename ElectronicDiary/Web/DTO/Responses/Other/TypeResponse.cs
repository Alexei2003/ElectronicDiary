﻿namespace ElectronicDiary.Web.DTO.Responses.Other
{
    public record TypeResponse : BaseResponse
    {
        public string? Name { get; init; } = null;
    }
}
