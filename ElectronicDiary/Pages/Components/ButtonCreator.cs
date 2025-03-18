using ElectronicDiary.SaveData;

namespace ElectronicDiary.Pages.Components
{
    public static class ButtonCreator
    {
        public static Button Create(string text, EventHandler handler)
        {
            var button = new Button()
            {
                // Положение
                HorizontalOptions = LayoutOptions.Fill,

                // Цвета
                BackgroundColor = UserData.UserSettings.Colors.ACCENT_COLOR,
                TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                // Текст
                FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                Text = text,
            };

            button.Clicked += handler;
            return button;
        }
    }
}
