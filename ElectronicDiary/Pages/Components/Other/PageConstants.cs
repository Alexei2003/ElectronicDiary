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

        // URL
        public const string NO_IMAGE_URL = "http://77.222.37.9/files/base/no_image.jpg";
    }
}
