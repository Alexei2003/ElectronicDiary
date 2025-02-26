using ElectronicDiary.Pages.Otherts;
using ElectronicDiary.SaveData;

namespace ElectronicDiary.Pages
{
    public class SettignsPage : ContentPage
    {
        public SettignsPage()
        {
            Title = "Настройки";

            // Цвета
            BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_PAGE_COLOR;

            Content = new VerticalStackLayout
            {
                // Положение
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Fill,
                Padding = PageConstants.PADDING_ALL_PAGES,
                Spacing = PageConstants.SPACING_ALL_PAGES,

                Children =
                {
                }
            };
        }
    }
}
