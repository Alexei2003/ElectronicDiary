namespace ElectronicDiary.SaveData.Other
{
    public class UserInfo
    {
        public long Id { get; set; } = -1;
        public string? Role { get; set; } = null;

        // Авторизация
        public string? Login { get; set; } = null;
        public string? Password { get; set; } = null;
    }
}
