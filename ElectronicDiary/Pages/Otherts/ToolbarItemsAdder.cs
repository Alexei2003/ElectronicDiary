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
            #elif OTHER_PLATFORM
                item.IconImageSource = ImageSource.FromFile(imagePath);               
            #endif

            item.Clicked += eventHandler;
            toolbarItems.Add(item);
        }



        public static void AddSettings(IList<ToolbarItem> toolbarItems)
        {
            AddElem(toolbarItems, "Настройки", "settings_icon.svg", SettingsClicked);
        }
        private async static void SettingsClicked(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PushAsync(new SettignsPage());
        }



        public static void AddLogOut(IList<ToolbarItem> toolbarItems)
        {
            AddElem(toolbarItems, "Выход", "log_out_icon.svg", LogOutClicked);
        }
        private async static void LogOutClicked(object sender, EventArgs e)
        {
            var response = await Sessions.logOut();
            if (!response.Error)
            {
                UserData.UserInfo = new UserInfo();
                UserData.SaveUserInfo();

                Application.Current.MainPage = new ThemedNavigationPage(new LogPage());
            }
        }
    }
}