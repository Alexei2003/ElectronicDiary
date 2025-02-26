using ElectronicDiary.Pages.Otherts;
using ElectronicDiary.SaveData;

namespace ElectronicDiary.Pages
{
    class SettignsPage : ContentPage
    {
        public SettignsPage()
        {
            Title = "Настройки";
            ToolbarItemsAdder.AddLogOut(ToolbarItems);

            // Цвета
            BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_PAGE_COLOR;

            Content = new VerticalStackLayout
            {
                Padding = PageConstants.PADDING_ALL_PAGES,
                Spacing = PageConstants.SPACING_ALL_PAGES,
                Children =
                {
                }
            };
        }
    }
}
