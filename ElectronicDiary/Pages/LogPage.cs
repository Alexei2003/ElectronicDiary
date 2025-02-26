using ElectronicDiary.Pages.Otherts;
using ElectronicDiary.SaveData;
using ElectronicDiary.SaveData.SerializeClasses;
using ElectronicDiary.Web.Api;

namespace ElectronicDiary.Pages
{
    public class LogPage : ContentPage
    {
        private string _login = "";
        private string _password = "";

        public LogPage()
        {
            Title = "Авторизация";
            ToolbarItemsAdder.AddSettings(ToolbarItems);

            // Цвета
            BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_PAGE_COLOR;

            var loginField = new Entry
            {
                // Положение
                HorizontalOptions = LayoutOptions.Fill,

                // Цвета
                BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_FILL_COLOR,
                TextColor = UserData.UserSettings.Colors.TEXT_COLOR,
                PlaceholderColor = UserData.UserSettings.Colors.PLACEHOLDER_COLOR,

                // Текст
                FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
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

                // Цвета
                BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_FILL_COLOR,
                TextColor = UserData.UserSettings.Colors.TEXT_COLOR,
                PlaceholderColor = UserData.UserSettings.Colors.PLACEHOLDER_COLOR,

                // Текст
                FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
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

                // Цвета
                BackgroundColor = UserData.UserSettings.Colors.ACCENT_COLOR,
                TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                // Текст
                FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                Text = "Вход",
            };
            toProfilePageButton.Clicked += ToProfilePageButtonClicked;

            Content = new VerticalStackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Padding = PageConstants.PADDING_ALL_PAGES,
                Spacing = PageConstants.SPACING_ALL_PAGES,
                Children =
                {
                    loginField,
                    passwordField,
                    toProfilePageButton
                }
            };
        }

        private async void ToProfilePageButtonClicked(object? sender, EventArgs e)
        {
            var response = await Sessions.LogIn(_login, _password);
            if (response.Error)
            {
                await DisplayAlert("Ошибка", response.Message, "OK");
                //if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                //{
                //    Navigation.PushAsync(new LogPage());
                //}
            }
            else
            {
                UserData.UserInfo = new UserInfo()
                {
                    Role = response.Message,
                    Login = _login,
                    Password = _password
                };
                UserData.SaveUserInfo();
                Application.Current.MainPage = new ThemedNavigationPage(new AdminPage());
            }
        }
    }
}
