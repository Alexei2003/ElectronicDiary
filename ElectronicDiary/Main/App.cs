using System.Text.Json;

using ElectronicDiary.Pages;
using ElectronicDiary.Pages.OtherPages;
using ElectronicDiary.SaveData.Other;
using ElectronicDiary.SaveData.Static;
using ElectronicDiary.UI.Components.Navigation;
using ElectronicDiary.UI.Components.Other;
using ElectronicDiary.Web.Api.Other;
using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Main
{
    public partial class App : Application
    {
        protected override Window CreateWindow(IActivationState? activationState)
        {
            UserData.LoadAll();

            var window = new Window(new EmptyPage())
            {
                Title = "Электронный дневник"
            };

            Task.Run(async () =>
            {
                if (!string.IsNullOrEmpty(UserData.UserInfo.Login) && !string.IsNullOrEmpty(UserData.UserInfo.Password))
                {
                    var response = await AuthorizationСontroller.LogIn(UserData.UserInfo.Login, UserData.UserInfo.Password);
                    if (!string.IsNullOrEmpty(response))
                    {
                        response = await AuthorizationСontroller.GetUserInfo();
                        if (!string.IsNullOrEmpty(response))
                        {
                            var obj = JsonSerializer.Deserialize<AuthorizationUserResponse>(response, PageConstants.JsonSerializerOptions);
                            if (obj != null)
                            {
                                if ((obj.Id ?? 1) == UserData.UserInfo.Id && UserInfo.ConverStringRoleToEnum(obj.Role) == UserData.UserInfo.Role)
                                {
                                    await Task.Delay(3000);
                                    Navigator.ChooseRootPageByRole(UserData.UserInfo.Role, UserData.UserInfo.Id);
                                    return;
                                }
                            }
                        }
                    }
                }

                await Task.Delay(3000);
                Navigator.SetAsRoot(new LogPage());
            });

            return window;
        }
    }
}