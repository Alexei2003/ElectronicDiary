using System.Text.Json;

namespace ElectronicDiary.SaveData
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
                if (obj != null)
                {
                    UserInfo = obj;
                }
            }
        }



        private static readonly string USER_SETTINGS_PATH = Path.Combine(FileSystem.AppDataDirectory, "UserSettings.ed");
        public static UserSettings UserSettings { get; set; } = new();

        public static void SaveUserSettings()
        {
            var json = JsonSerializer.Serialize(UserSettings);
            File.WriteAllText(USER_SETTINGS_PATH, json);
        }

        public static void LoadUserSettings()
        {
            if (File.Exists(USER_SETTINGS_PATH))
            {
                var json = File.ReadAllText(USER_SETTINGS_PATH);
                var obj = JsonSerializer.Deserialize<UserSettings>(json);
                if (obj != null)
                {
                    UserSettings = obj;
                }
            }
        }
    }
}
