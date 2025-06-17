using System.Text.Json;

using ElectronicDiary.SaveData.Other;
using ElectronicDiary.SaveData.Static;
using ElectronicDiary.UI.Components.Elems;
using ElectronicDiary.UI.Components.Navigation;
using ElectronicDiary.UI.Components.Other;
using ElectronicDiary.UI.Views.Lists.General;
using ElectronicDiary.Web.Api.Other;
using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Pages
{
    public partial class LogPage : ContentPage
    {
        private string _login = string.Empty;
        private string _password = string.Empty;

        public LogPage()
        {
            Title = "Авторизация";
            ToolbarItemsAdder.AddSettings(ToolbarItems);
            BackgroundColor = UserData.Settings.Theme.BackgroundPageColor;

            var loginEntry = BaseElemsCreator.CreateEditor(newText => _login = newText, "Логин");

            var passwordEntry = new Entry
            {
                BackgroundColor = UserData.Settings.Theme.AccentColorFields,
                TextColor = UserData.Settings.Theme.TextColor,
                PlaceholderColor = UserData.Settings.Theme.PlaceholderColor,

                FontSize = UserData.Settings.Fonts.BaseFontSize,
                Placeholder = "Пароль",
                IsPassword = true
            };

            passwordEntry.TextChanged += (sender, e) => _password = e.NewTextValue;

            var toProfilePageButton = BaseElemsCreator.CreateButton("Вход", ToProfilePageButtonClicked);

            var vStack = BaseElemsCreator.CreateVerticalStackLayout();
            Application.Current?.Dispatcher.Dispatch(() =>
            {
                AdminPageStatic.CalcViewWidth(out double width, out _);
                vStack.MaximumWidthRequest = width;
            });
            vStack.Add(loginEntry);
            vStack.Add(passwordEntry);
            vStack.Add(toProfilePageButton);
            Content = vStack;
        }

        private async void ToProfilePageButtonClicked(object? sender, EventArgs e)
        {
            var response = await AuthorizationСontroller.LogIn(_login.Trim(), _password.Trim());
            if (response != null)
            {
                response = await AuthorizationСontroller.GetUserInfo();
                if (!string.IsNullOrEmpty(response))
                {
                    var obj = JsonSerializer.Deserialize<AuthorizationUserResponse>(response, PageConstants.JsonSerializerOptions);
                    if (obj != null)
                    {
                        UserData.UserInfo = new UserInfo()
                        {
                            Id = obj.Id ?? 1,
                            UserId = obj.UserId,
                            Role = UserInfo.ConverStringRoleToEnum(obj.Role),
                            Login = _login,
                            Password = _password
                        };
                        UserData.SaveUserInfo();
                        Navigator.ChooseRootPageByRole(UserData.UserInfo.Role, UserData.UserInfo.Id);
                    }
                }
            }
        }
    }
}
