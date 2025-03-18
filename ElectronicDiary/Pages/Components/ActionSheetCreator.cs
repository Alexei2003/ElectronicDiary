namespace ElectronicDiary.Pages.Components
{
    public static class ActionSheetCreator
    {
        public static async Task<string> Create(string[] actionList)
        {
            var page = Application.Current?.Windows[0].Page;
            var action = string.Empty;
            if (page != null) action = await page.DisplayActionSheet(
                "Выберите действие",    // Заголовок
                "Отмена",               // Кнопка отмены
                null,                   // Кнопка деструктивного действия (например, удаление)
                actionList);            // Остальные кнопки

            return action;
        }
    }
}
