using System.Text.Json;

namespace ElectronicDiary.Pages.Components.Other
{
    public static class PageConstants
    {
        // Json
        public static JsonSerializerOptions JsonSerializerOptions { get; set; } = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
}
