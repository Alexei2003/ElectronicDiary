namespace ElectronicDiary.SaveData.Other
{
    public class UserInfo
    {
        public long Id { get; set; } = -1;
        public long UserId { get; set; } = -1;
        public RoleType Role { get; set; } = RoleType.None;
        public long EducationId { get; set; } = -1;

        // Авторизация
        public string? Login { get; set; } = null;
        public string? Password { get; set; } = null;

        public enum RoleType
        {
            None, MainOperator, LocalOperator, Teacher, SchoolStudent, Parent, Administration
        }

        public static RoleType ConverStringRoleToEnum(string? role)
        {
            return role switch
            {
                "Main admin" => RoleType.MainOperator,
                "Local admin" => RoleType.LocalOperator,
                "Teacher" => RoleType.Teacher,
                "School student" => RoleType.SchoolStudent,
                "Parent" => RoleType.Parent,
                "Administration" => RoleType.Administration,
                _ => RoleType.None,
            };
        }

        public static string ConvertEnumRoleToStringRus(RoleType role)
        {
            return role switch
            {
                RoleType.MainOperator => "Главный оператор",
                RoleType.LocalOperator => "Школьный оператор",
                RoleType.Teacher => "Учитель",
                RoleType.SchoolStudent => "Ученик школы",
                RoleType.Parent => "Родитель",
                RoleType.Administration => "Администрация школьная",
                _ => "Нет"
            };
        }
    }
}
