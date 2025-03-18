using ElectronicDiary.Pages.Others;
using ElectronicDiary.SaveData;

namespace ElectronicDiary.Pages.Otherts
{
    public partial class TestPage : ContentPage
    {
        public TestPage()
        {
            var namePageLabel = new Label
            {
                // Положение
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,

                // Текст
                TextColor = UserData.UserSettings.Colors.TEXT_COLOR,
                FontSize = 20,
                Text = "GOOD",
            };

            Content = new VerticalStackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Padding = PageConstants.PADDING_ALL_PAGES,
                Spacing = 10,
                Children =
                {
                    namePageLabel
                }
            };
        }
    }
}
