using ElectronicDiary.Pages.AdminPageComponents;

namespace ElectronicDiary.Pages
{
    public partial class EmptyPage : ContentPage
    {
        public EmptyPage()
        {
            Navigation.PushAsync(new AdminPage());
        }
    }
}