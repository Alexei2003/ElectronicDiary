using ElectronicDiary.SaveData.Themes;

namespace ElectronicDiary.SaveData.Other
{
    public class Settings
    {
        public UserSettingsClass UserSettings { get; set; }
        public SizesClass Sizes { get; set; }
        public FontsClass Fonts { get; set; }
        public IAppTheme Theme { get; set; }

        public Settings()
        {
            UserSettings = new();
            Sizes = new(UserSettings.ScaleFactor);
            Fonts = new FontsClass(UserSettings.ScaleFactor);
            Theme = ThemesMeneger.ChooseTheme(UserSettings.ThemeIndex);
        }


        public class UserSettingsClass
        {
            public float ScaleFactor { get; set; } = 1f;
            public long ThemeIndex { get; set; } = 0;
        }

        public class SizesClass
        {
            // Размеры
            public Thickness Padding { get; set; }
            public int Spacing { get; set; }
            public int Image { get; set; }
            public int ImageButton { get; set; }
            public int CellWidthText { get; set; }
            public int CellWidthScore { get; set; }
            public int CellHeightScore { get; set; }
            public SizesClass(float scaleFactor)
            {
                Padding = (int)(scaleFactor * UserSettingConstants.SizesClass.Padding);
                Spacing = (int)(scaleFactor * UserSettingConstants.SizesClass.Spacing);
                Image = (int)(scaleFactor * UserSettingConstants.SizesClass.Image);
                ImageButton = (int)(scaleFactor * UserSettingConstants.SizesClass.ImageButton);
                CellWidthText = (int)(scaleFactor * UserSettingConstants.SizesClass.CellWidthText);
                CellWidthScore = (int)(scaleFactor * UserSettingConstants.SizesClass.CellWidthScore);
            }
        }

        public class FontsClass
        {
            public int TitleFontSize { get; set; }
            public int BaseFontSize { get; set; }

            public FontsClass(float scaleFactor)
            {
                TitleFontSize = (int)(scaleFactor * UserSettingConstants.FontsClass.TitleFontSize);
                BaseFontSize = (int)(scaleFactor * UserSettingConstants.FontsClass.BaseFontSize);
            }
        }
    }
}
