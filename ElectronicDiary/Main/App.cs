using System.Text.Json;

using ElectronicDiary.Pages;
using ElectronicDiary.Pages.Components.Navigation;
using ElectronicDiary.Pages.Components.Other;
using ElectronicDiary.Pages.OtherPages;
using ElectronicDiary.SaveData.Other;
using ElectronicDiary.SaveData.Static;
using ElectronicDiary.Web.Api.Other;
using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Main
{
    public partial class App : Application
    {
        protected override Window CreateWindow(IActivationState? activationState)
        {
            UserData.LoadAll();

            Task.Run(async () =>
            {
                await Task.Delay(1000);
                await AuthorizationСontroller.LogIn(UserData.UserInfo.Login ?? string.Empty, UserData.UserInfo.Password ?? string.Empty);
                var response = await AuthorizationСontroller.GetUserInfo();
                if (!string.IsNullOrEmpty(response))
                {
                    var obj = JsonSerializer.Deserialize<AuthorizationUserResponse>(response, PageConstants.JsonSerializerOptions);
                    if (obj != null)
                    {
                        if (obj.Id == UserData.UserInfo.Id && UserInfo.ConverStringRoleToEnum(obj.Role) == UserData.UserInfo.Role)
                        {
                            Navigator.ChoosePageByRole(UserData.UserInfo.Role, UserData.UserInfo.Id);
                            return;
                        }
                    }
                }

                Navigator.SetAsRoot(new LogPage());
            });

            return new Window(new EmptyPage())
            {
                Title = "Электронный дневник"
            };
        }
    }
}