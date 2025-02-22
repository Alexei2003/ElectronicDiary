using ElectronicDiary.Pages;

namespace ElectronicDiary
{
    public class App : Application
    {
        protected override Window CreateWindow(IActivationState? activationState)
        {
            var navigationPage = new NavigationPage(new LogPage());

            return new Window(navigationPage)
            {
                Title = "Электронный дневник" 
            };
        }
    }
}