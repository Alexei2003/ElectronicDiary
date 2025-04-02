using ElectronicDiary.Pages.Others;

namespace ElectronicDiary.Pages.Components.Navigation
{
    public static class Navigator
    {
        public static void SetAsRoot(ContentPage page)
        {
            if (Application.Current?.Windows.Count > 0)
            {
                Application.Current.Windows[0].Page = new ThemedNavigationPage(page);
            }
        }
    }
}
