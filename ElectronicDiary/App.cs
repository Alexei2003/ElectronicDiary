using ElectronicDiary.Pages;
using ElectronicDiary.SaveData;
using ElectronicDiary.Web;

namespace ElectronicDiary
{
    public class App : Application
    {
        protected override Window CreateWindow(IActivationState? activationState)
        {
            UserData.LoadAll();

            var startPage = new NavigationPage(new LogPage());

            Task.Run(async () =>
            {
                if (UserData.UserInfo.Login != null && UserData.UserInfo.Password != null)
                {
                    var response = await HttpClientCustom.LogIn(UserData.UserInfo.Login, UserData.UserInfo.Password);

                    if (!response.Error)
                    {
                        Dispatcher.Dispatch(() =>
                        {
                            startPage.Navigation.PushAsync(new TestPage());
                        });
                    }
                }
            });

            return new Window(startPage)
            {
                Title = "Электронный дневник"
            };
        }
    }
}