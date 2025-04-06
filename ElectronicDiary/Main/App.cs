using ElectronicDiary.Pages;
using ElectronicDiary.Pages.Others;
using ElectronicDiary.SaveData.Static;
using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary
{
    public partial class App : Application
    {
        protected override Window CreateWindow(IActivationState? activationState)
        {
            UserData.LoadAll();

            Task.Run(async () =>
            {
                await Task.Delay(1000);
                if (!string.IsNullOrEmpty(UserData.UserInfo.Role))
                {
                    var response = await AuthorizationСontroller.LogIn(UserData.UserInfo.Login ?? string.Empty, UserData.UserInfo.Password ?? string.Empty);

                    if (!string.IsNullOrEmpty(response))
                    {
                        Dispatcher.Dispatch(() =>
                        {
                            if (Current?.Windows.Count > 0)
                            {
                                Current.Windows[0].Page = new ThemedNavigationPage(new EmptyPage());
                            }
                        });
                    }
                }
            });

            return new Window(new LogPage())
            {
                Title = "Электронный дневник"
            };
        }
    }
}