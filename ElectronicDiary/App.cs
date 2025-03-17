using ElectronicDiary.Pages;
using ElectronicDiary.Pages.Otherts;
using ElectronicDiary.SaveData;
using ElectronicDiary.Web.Api;

namespace ElectronicDiary
{
    public partial class App : Application
    {
        protected override Window CreateWindow(IActivationState? activationState)
        {
            UserData.LoadAll();

            var startPage = new ThemedNavigationPage(new LogPage());

            Task.Run(async () =>
            {
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
;
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