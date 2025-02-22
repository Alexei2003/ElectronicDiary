using ElectronicDiary.Web;

namespace ElectronicDiary.Pages
{
    public class LogPage : ContentPage
    {
        private string _login = "";
        private string _password = "";

        public LogPage()
        {
            var namePageLabel = new Label
            {
                // Положение
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,

                // Текст
                TextColor = PageConstants.TEXR_COLOR,
                FontSize = 20,
                Text = "Вход",
            };

            var loginField = new Entry
            {
                // Положение
                HorizontalOptions = LayoutOptions.Fill,

                // Текст
                TextColor = PageConstants.TEXR_COLOR,
                PlaceholderColor = PageConstants.PLACEHOLDER_COLOR,
                Placeholder = "Логин",
            };
            loginField.TextChanged += (sender, e) =>
            {
                _login = e.NewTextValue;
            };

            var passwordField = new Entry
            {
                // Положение
                HorizontalOptions = LayoutOptions.Fill,

                // Текст
                TextColor = PageConstants.TEXR_COLOR,
                PlaceholderColor = PageConstants.PLACEHOLDER_COLOR,
                Placeholder = "Пароль",
            };
            passwordField.TextChanged += (sender, e) =>
            {
                _password = e.NewTextValue;
            };

            var toProfilePageButton = new Button
            {
                // Положение
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,

                // Текст
                TextColor = PageConstants.TEXR_COLOR,
                Text = "Вход"
            };
            toProfilePageButton.Clicked += ButtonClickedToProfilePage;

            Content = new VerticalStackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Padding = new Thickness(PageConstants.PADDING_ALL_PAGES),
                Spacing = 10,
                Children =
                {
                    namePageLabel,
                    loginField,
                    passwordField,
                    toProfilePageButton
                }
            };
        }

        private async void ButtonClickedToProfilePage(object? sender, EventArgs e)
        {
            var response = await HttpClientCustom.LogIn(_login, _password);
            if (response.Error == true)
            {
                await DisplayAlert("Ошибка", response.Message, "OK");
            }
        }
    }
}
