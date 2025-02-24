using ElectronicDiary.Web;

namespace ElectronicDiary.Pages
{
    public class TestPage : ContentPage
    {
        public TestPage()
        {
            var namePageLabel = new Label
            {
                // Положение
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,

                // Текст
                TextColor = PageConstants.TEXR_COLOR,
                FontSize = 20,
                Text = "GOOD",
            };

            Content = new VerticalStackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(PageConstants.PADDING_ALL_PAGES),
                Spacing = 10,
                Children =
                {
                    namePageLabel
                }
            };
        }
    }
}
