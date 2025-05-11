namespace ElectronicDiary.SaveData.Other
{
    public class UserInfo
    {
        public long Id { get; set; } = -1;
        public RoleType Role { get; set; } = RoleType.None;
        public long EducationId { get; set; } = -1;

        // Авторизация
        public string? Login { get; set; } = null;
        public string? Password { get; set; } = null;

        public enum RoleType
        {
            None, MainAdmin, LocalAdmin, Teacher, SchoolStudent, Parent, Administration, Ministry
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
                "Administration" => RoleType.Administration,
                "Ministry" => RoleType.Ministry,
                _ => RoleType.None,
            };
        }

        public static string ConvertEnumRoleToString(RoleType role)
        {
            return role switch
            {
                RoleType.MainAdmin => "Main admin",
                RoleType.LocalAdmin => "Local admin",
                RoleType.Teacher => "Teacher",
                RoleType.SchoolStudent => "School student",
                RoleType.Parent => "Parent",
                RoleType.Administration => "Administration",
                RoleType.Ministry => "Ministry",
                _ => "None"
            };
        }

        public static string ConvertEnumRoleToStringRus(RoleType role)
        {
            return role switch
            {
                RoleType.MainAdmin => "Главный администратор",
                RoleType.LocalAdmin => "Локальный администратор",
                RoleType.Teacher => "Учитель",
                RoleType.SchoolStudent => "Ученик школы",
                RoleType.Parent => "Родитель",
                RoleType.Administration => "Администрация школьная",
                RoleType.Ministry => "Администрация министерства",
                _ => "Нет"
            };
        }
    }
}
