using System.Text.Json;

using ElectronicDiary.Pages.AdminPageComponents.General;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Pages.Components.Navigation;
using ElectronicDiary.Pages.Components.Other;
using ElectronicDiary.SaveData.Other;
using ElectronicDiary.SaveData.Static;
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

            var loginEntry = BaseElemsCreator.CreateEntry(newText => _login = newText, "Логин");

            var passwordEntry = BaseElemsCreator.CreateEntry(newText => _password = newText, "Пароль");
            passwordEntry.IsPassword = true;

            var toProfilePageButton = BaseElemsCreator.CreateButton("Вход", ToProfilePageButtonClicked);

            var vStack = BaseElemsCreator.CreateVerticalStackLayout();
            AdminPageStatic.CalcViewWidth(out double width, out _);
            vStack.MaximumWidthRequest = width;
            vStack.Add(loginEntry);
            vStack.Add(passwordEntry);
            vStack.Add(toProfilePageButton);
            Content = vStack;
        }

        private async void ToProfilePageButtonClicked(object? sender, EventArgs e)
        {
            await AuthorizationСontroller.LogIn(_login, _password);
            var response = await AuthorizationСontroller.GetUserInfo();
            if (!string.IsNullOrEmpty(response))
            {
                var obj = JsonSerializer.Deserialize<AuthorizationUserResponse>(response, PageConstants.JsonSerializerOptions);
                if (obj != null)
                {
                    UserData.UserInfo = new UserInfo()
                    {
                        Id = obj.Id,
                        Role = obj.Role,
                        Login = _login,
                        Password = _password
                    };
                    UserData.SaveUserInfo();
                    Navigator.ChoosePage(UserData.UserInfo.Role, UserData.UserInfo.Id);
                }
            }
        }
    }
}
