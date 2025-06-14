namespace ElectronicDiary.SaveData.Other
{
    public class UserSettingConstants
    {
        public SizesClass Sizes { get; set; } = new();

        public FontsClass Fonts { get; set; } = new();

        public class SizesClass
        {
            // Размеры
            public const int Padding = 10;
            public const int Spacing = 10;
            public const int Image = 100;
            public const int ImageButton = 50;
            public const int CellWidthText = 140;
            public const int CellWidthScore = 40;
        }

        public class FontsClass
        {
            public const int TitleFontSize = 16;
            public const int BaseFontSize = 12;
        }
    }
}
