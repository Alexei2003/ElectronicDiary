using ElectronicDiary.SaveData.Static;

namespace ElectronicDiary.Pages.Others
{
    public partial class ThemedNavigationPage : NavigationPage
    {
        // Красит полосу навигации
        public ThemedNavigationPage(Page root) : base(root)
        {
            BarBackgroundColor = UserData.UserSettings.Colors.NAVIGATION_PAGE_COLOR;
            BarTextColor = UserData.UserSettings.Colors.TEXT_COLOR;
        }
    }
}