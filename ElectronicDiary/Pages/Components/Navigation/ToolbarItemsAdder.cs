﻿using ElectronicDiary.Pages.OtherPages;
using ElectronicDiary.Pages.Others;
using ElectronicDiary.SaveData.Other;
using ElectronicDiary.SaveData.Static;
using ElectronicDiary.Web.Api.Other;

namespace ElectronicDiary.Pages.Components.Navigation
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
            AddElem(toolbarItems, "Настройки", UserData.Settings.Theme.TextIsBlack ? "black_settings_icon.png" : "white_settings_icon.png", SettingsClicked);
        }
        private static void SettingsClicked(object? sender, EventArgs e)
        {
            var page = Application.Current?.Windows[0].Page;
            page?.Navigation.PushAsync(new SettignsPage());
        }



        public static void AddLogOut(IList<ToolbarItem> toolbarItems)
        {
            AddElem(toolbarItems, "Выход", UserData.Settings.Theme.TextIsBlack ? "black_log_out_icon.png" : "white_log_out_icon.png", LogOutClicked);
        }
        private async static void LogOutClicked(object? sender, EventArgs e)
        {
            var response = await AuthorizationСontroller.LogOut();
            if (!string.IsNullOrEmpty(response))
            {
                UserData.UserInfo = new UserInfo();
                UserData.SaveUserInfo();

                if (Application.Current?.Windows.Count > 0) Application.Current.Windows[0].Page = new ThemedNavigationPage(new LogPage());
            }
        }
    }
}