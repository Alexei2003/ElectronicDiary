using ElectronicDiary.SaveData;

namespace ElectronicDiary.Pages.Otherts
{
    public partial class ThemedNavigationPage : NavigationPage
    {
        public ThemedNavigationPage(Page root) : base(root)
        {
            BarBackgroundColor = UserData.UserSettings.Colors.NAVIGATION_PAGE_COLOR;
            BarTextColor = UserData.UserSettings.Colors.TEXT_COLOR;
        }
    }
}