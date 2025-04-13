using System.Text.Json;

using ElectronicDiary.SaveData.Other;
using ElectronicDiary.SaveData.Themes;

namespace ElectronicDiary.SaveData.Static
{
    public static class UserData
    {
        //public static void SaveAll()
        //{
        //    SaveUserInfo();
        //    LoadUserSettings();
        //}

        public static void LoadAll()
        {
            LoadUserInfo();
            LoadUserSettings();
        }

        private static readonly string USER_INFO_PATH = Path.Combine(FileSystem.AppDataDirectory, "UserInfo.ed");
        public static UserInfo UserInfo { get; set; } = new();
        public static void SaveUserInfo()
        {
            var json = JsonSerializer.Serialize(UserInfo);
            File.WriteAllText(USER_INFO_PATH, json);
        }

        public static void LoadUserInfo()
        {
            if (File.Exists(USER_INFO_PATH))
            {
                var json = File.ReadAllText(USER_INFO_PATH);
                var obj = JsonSerializer.Deserialize<UserInfo>(json);
                if (obj != null) UserInfo = obj;
            }
        }



        private static readonly string USER_SETTINGS_PATH = Path.Combine(FileSystem.AppDataDirectory, "UserSettings.ed");
        public static Settings Settings { get; set; } = new();

        public static void SaveUserSettings()
        {
            var json = JsonSerializer.Serialize(Settings.UserSettings);
            File.WriteAllText(USER_SETTINGS_PATH, json);
        }

        public static void LoadUserSettings()
        {
            if (File.Exists(USER_SETTINGS_PATH))
            {
                var json = File.ReadAllText(USER_SETTINGS_PATH);
                var obj = JsonSerializer.Deserialize<Settings.UserSettingsClass>(json);
                if (obj != null)
                {
                    Settings.UserSettings = obj;
                    Settings.Sizes = new(obj.ScaleFactor);
                    Settings.Fonts = new(obj.ScaleFactor);
                    Settings.Theme = ThemesMeneger.ChooseTheme(obj.ThemeIndex);
                }
            }
        }
    }
}
