using ElectronicDiary.Pages.AdminPageComponents.General;
using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.Pages.Components.Navigation;
using ElectronicDiary.Pages.Components.NavigationPage;
using ElectronicDiary.SaveData.SerializeClasses;
using ElectronicDiary.SaveData.Static;
using ElectronicDiary.Web.Api.Other;

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
            BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_PAGE_COLOR;

            var loginEntry = BaseElemsCreator.CreateEntry(newText => _login = newText, "Логин");

            var passwordEntry = BaseElemsCreator.CreateEntry(newText => _password = newText, "Пароль");
            passwordEntry.IsPassword = true;

            var toProfilePageButton = BaseElemsCreator.CreateButton("Вход", ToProfilePageButtonClicked);

            var vStack = BaseElemsCreator.CreateVerticalStackLayout();
            vStack.Add(loginEntry);
            vStack.Add(passwordEntry);
            vStack.Add(toProfilePageButton);
            AdminPageStatic.CalcViewWidth(vStack);
            Content = vStack;
        }

        private async void ToProfilePageButtonClicked(object? sender, EventArgs e)
        {
            var response = await AuthorizationСontroller.LogIn(_login, _password);
            if (!string.IsNullOrEmpty(response))
            {
                UserData.UserInfo = new UserInfo()
                {
                    Id = 21,
                    Role = response,
                    Login = _login,
                    Password = _password
                };
                UserData.SaveUserInfo();
                Navigator.ChoosePage(response, UserData.UserInfo.Id);
            }
        }
    }
}
