using ElectronicDiary.Pages.AdminPageComponents;

namespace ElectronicDiary.Pages.Others
{
    public partial class EmptyPage : ContentPage
    {
        public EmptyPage()
        {
            Navigation.PushAsync(new AdminPage());
        }
    }
}