using System.Text.Json;

namespace ElectronicDiary.SaveData.Other
{
    public class UserInfo
    {
        public long Id { get; set; } = -1;
        public RoleType Role { get; set; } = RoleType.None;

        // Авторизация
        public string? Login { get; set; } = null;
        public string? Password { get; set; } = null;

        public enum RoleType
        {
            None, MainAdmin, LocalAdmin, Teacher, SchoolStudent, Parent
        }

        public static RoleType ConverStringRoleToEnum(string? role)
        {
            return role switch
            {
                "Main admin" => RoleType.MainAdmin,
                "Local admin" => RoleType.LocalAdmin,
                "Teacher" => RoleType.Teacher,
                "School student" => RoleType.SchoolStudent,
                "Parent" => RoleType.Parent,
                _ => RoleType.None,
            };
        }
    }
}
