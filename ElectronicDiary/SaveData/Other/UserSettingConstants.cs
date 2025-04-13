namespace ElectronicDiary.SaveData.Other
{
    public class UserSettingConstants
    {
        public SizesClass Sizes { get; set; } = new();

        public FontsClass Fonts { get; set; } = new();

        public class SizesClass
        {
            // Размеры
            public const int PADDING_ALL_PAGES = 10;
            public const int SPACING_ALL_PAGES = 10;
            public const int IMAGE_SIZE = 100;
            public const int IMAGE_BUTTON_SIZE = 50;
        }

        public class FontsClass
        {
            public const int TITLE_FONT_SIZE = 16;
            public const int BASE_FONT_SIZE = 12;
        }
    }
}
