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
            public Thickness PADDING_ALL_PAGES = 10;
            public int SPACING_ALL_PAGES = 10;
            public int IMAGE_SIZE = 100;
            public int IMAGE_BUTTON_SIZE = 50;

            public SizesClass(float scaleFactor)
            {
                PADDING_ALL_PAGES = (int)(scaleFactor * UserSettingConstants.SizesClass.PADDING_ALL_PAGES);
                SPACING_ALL_PAGES = (int)(scaleFactor * UserSettingConstants.SizesClass.SPACING_ALL_PAGES);
                IMAGE_SIZE = (int)(scaleFactor * UserSettingConstants.SizesClass.IMAGE_SIZE);
                IMAGE_BUTTON_SIZE = (int)(scaleFactor * UserSettingConstants.SizesClass.IMAGE_BUTTON_SIZE);
            }
        }

        public class FontsClass
        {
            public int TITLE_FONT_SIZE { get; set; } = 16;
            public int BASE_FONT_SIZE { get; set; } = 12;

            public FontsClass(float scaleFactor)
            {
                TITLE_FONT_SIZE = (int)(scaleFactor * UserSettingConstants.FontsClass.TITLE_FONT_SIZE);
                BASE_FONT_SIZE = (int)(scaleFactor * UserSettingConstants.FontsClass.BASE_FONT_SIZE);
            }
        }
    }
}
