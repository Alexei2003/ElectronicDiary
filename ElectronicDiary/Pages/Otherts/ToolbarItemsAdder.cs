using ElectronicDiary.SaveData;
using ElectronicDiary.SaveData.SerializeClasses;
using ElectronicDiary.Web.Api;

namespace ElectronicDiary.Pages.Otherts
{
    public static class ToolbarItemsAdder
    {
        private static void AddElem(IList<ToolbarItem> toolbarItems, string title, string imagePath, EventHandler eventHandler)
        {
            var item = new ToolbarItem();

#if WINDOWS
            item.Text = title;
#else
            item.IconImageSource = ImageSource.FromFile(imagePath);
#endif

            item.Clicked += eventHandler;
            toolbarItems.Add(item);
        }



        public static void AddSettings(IList<ToolbarItem> toolbarItems)
        {
            AddElem(toolbarItems, "Настройки", "settings_icon.svg", SettingsClicked);
        }
        private static void SettingsClicked(object? sender, EventArgs e)
        {
            var page = Application.Current?.Windows[0].Page;
            if (page != null) page.Navigation.PushAsync(new SettignsPage());
        }



        public static void AddLogOut(IList<ToolbarItem> toolbarItems)
        {
            AddElem(toolbarItems, "Выход", "log_out_icon.svg", LogOutClicked);
        }
        private async static void LogOutClicked(object? sender, EventArgs e)
        {
            var response = await AuthorizationСontroller.LogOut();
            if (response != null)
            {
                UserData.UserInfo = new UserInfo();
                UserData.SaveUserInfo();

                if (Application.Current?.Windows.Count > 0) Application.Current.Windows[0].Page = new ThemedNavigationPage(new LogPage());
            }
        }
    }
}