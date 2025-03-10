using ElectronicDiary.Pages;
using ElectronicDiary.Pages.Otherts;
using ElectronicDiary.SaveData;
using ElectronicDiary.Web.Api;

namespace ElectronicDiary
{
    public class App : Application
    {
        protected override Window CreateWindow(IActivationState? activationState)
        {
            UserData.LoadAll();

            var startPage = new ThemedNavigationPage(new LogPage());

            Task.Run(async () =>
            {
                if (UserData.UserInfo.Role != null)
                {
                    var response = await AuthorizationСontroller.LogIn(UserData.UserInfo.Login ?? "", UserData.UserInfo.Password ?? "");

                    if (response != null)
                    {
                        Dispatcher.Dispatch(() =>
                        {
                            Application.Current.Windows[0].Page = new ThemedNavigationPage(new EmptyPage());
                        });
                    }
                }
            });

            Thread.Sleep(TimeSpan.FromSeconds(1));

            return new Window(startPage)
            {
                Title = "Электронный дневник"
            };
        }
    }
}