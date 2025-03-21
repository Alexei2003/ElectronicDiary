﻿using System.Text.Json;

namespace ElectronicDiary.Pages.Others
{
    public static class PageConstants
    {
        // Размеры
        public static Thickness PADDING_ALL_PAGES { get; set; } = new Thickness(10);
        public const int SPACING_ALL_PAGES = 10;
        public const int IMAGE_SIZE = 50;

        // Json
        public static JsonSerializerOptions JsonSerializerOptions { get; set; } = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };


    }
}
