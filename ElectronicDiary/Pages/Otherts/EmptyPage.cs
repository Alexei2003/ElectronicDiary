using ElectronicDiary.Pages.AdminPageComponents;

namespace ElectronicDiary.Pages
{
    public class EmptyPage : ContentPage
    {
        public EmptyPage()
        {
            Navigation.PushAsync(new AdminPage());
        }
    }
}