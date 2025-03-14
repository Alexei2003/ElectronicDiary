﻿using ElectronicDiary.Pages.Otherts;
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

            var loginEntry = new Entry
            {
                // Цвета
                BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_FILL_COLOR,
                TextColor = UserData.UserSettings.Colors.TEXT_COLOR,
                PlaceholderColor = UserData.UserSettings.Colors.PLACEHOLDER_COLOR,

                // Текст
                FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                Placeholder = "Логин",
            };
            loginEntry.TextChanged += (sender, e) => _login = e.NewTextValue;

            var passwordEntry = new Entry
            {
                // Цвета
                BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_FILL_COLOR,
                TextColor = UserData.UserSettings.Colors.TEXT_COLOR,
                PlaceholderColor = UserData.UserSettings.Colors.PLACEHOLDER_COLOR,

                // Текст
                FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                Placeholder = "Пароль",
                IsPassword = true,
            };
            passwordEntry.TextChanged += (sender, e) => _password = e.NewTextValue;

            var toProfilePageButton = new Button
            {
                // Положение
                HorizontalOptions = LayoutOptions.Center,

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
                // Положение
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Fill,
                Padding = PageConstants.PADDING_ALL_PAGES,
                Spacing = PageConstants.SPACING_ALL_PAGES,

                Children =
                {
                    loginEntry,
                    passwordEntry,
                    toProfilePageButton
                }
            };
        }

        private async void ToProfilePageButtonClicked(object? sender, EventArgs e)
        {
            var response = await AuthorizationСontroller.LogIn(_login, _password);
            if (response != null)
            {
                UserData.UserInfo = new UserInfo()
                {
                    Role = response,
                    Login = _login,
                    Password = _password
                };
                UserData.SaveUserInfo();
                Application.Current.Windows[0].Page = new ThemedNavigationPage(new EmptyPage());
            }
        }
    }
}
