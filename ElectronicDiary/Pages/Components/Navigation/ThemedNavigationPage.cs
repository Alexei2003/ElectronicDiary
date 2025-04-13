using ElectronicDiary.SaveData.Static;

namespace ElectronicDiary.Pages.Others
{
    public partial class ThemedNavigationPage : NavigationPage
    {
        // Красит полосу навигации
        public ThemedNavigationPage(Page root) : base(root)
        {
            BarBackgroundColor = UserData.Settings.Theme.NavigationPageColor;
            BarTextColor = UserData.Settings.Theme.TextColor;
        }
    }
}