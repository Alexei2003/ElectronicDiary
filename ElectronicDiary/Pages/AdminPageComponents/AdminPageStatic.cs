using ElectronicDiary.SaveData;

namespace ElectronicDiary.Pages.AdminPageComponents
{
    public static class AdminPageStatic
    {
        // Тип вида
        public enum ViewType
        {
            EducationalInstitutionList, EducationalInstitution, LocalAdminList
        }

        // Обновление видов
        public static void RepaintPage(HorizontalStackLayout mainStack, List<ScrollView> viewList)
        {
            mainStack.Clear();

            double dpi = DeviceDisplay.MainDisplayInfo.Density * 160;

#if WINDOWS
            double coeff = 1;
            double widthLocal = Application.Current.Windows[0].Width;
#else
            double coeff = 2;
            double widthLocal = DeviceDisplay.MainDisplayInfo.Width;
#endif
            var countColumn = int.Min((int)(widthLocal / dpi / 2), 3);

            for (var i = int.Max(viewList.Count - countColumn, 0); i < viewList.Count; i++)
            {
                viewList[i].MaximumWidthRequest = (widthLocal / coeff / countColumn) * 0.90;
                mainStack.Add(viewList[i]);
            }
        }

        public static bool OnBackButtonPressed(HorizontalStackLayout mainStack, List<ScrollView> viewList)
        {
            if (viewList.Count > 1)
            {
                viewList.RemoveAt(viewList.Count - 1);
                RepaintPage(mainStack, viewList);
            }

            return true;
        }

        // Тип 2рого компонента
        public enum ComponentType
        {
            Label, Entity, Picker
        }

        // Добавление линий элементов в таблицах
        public sealed class ItemPicker()
        {
            public int Id { get; set; } = -1;
            public string Name { get; set; } = "";
        }

        public static object AddLineElems(ComponentType componentType,
                                         Grid grid,
                                         int startColumn,
                                         int startRow,
                                         string title,
                                         string? value = null,
                                         string? placeholder = null,
                                         Action<string>? textChangedAction = null,
                                         List<ItemPicker>? items = null,
                                         Action<int>? idChangedAction = null
                                         )
        {
            var titleLabel = new Label
            {
                // Цвета
                TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                // Текст
                FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                Text = title,
            };
            grid.Add(titleLabel, startColumn, startRow);

            switch (componentType)
            {
                case ComponentType.Label:
                    var valueLabel = new Label
                    {
                        // Цвета
                        TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                        // Текст
                        FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                        Text = value ?? "",
                    };

                    grid.Add(valueLabel, startColumn + 1, startRow);
                    return (object)valueLabel;
                case ComponentType.Entity:
                    var entryEntry = new Entry
                    {
                        // Цвета
                        BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_FILL_COLOR,
                        TextColor = UserData.UserSettings.Colors.TEXT_COLOR,
                        PlaceholderColor = UserData.UserSettings.Colors.PLACEHOLDER_COLOR,

                        // Текст
                        FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                        Placeholder = placeholder ?? ""
                    };
                    if (textChangedAction != null)
                    {
                        entryEntry.TextChanged += (sender, e) => textChangedAction(e.NewTextValue);
                    }

                    grid.Add(entryEntry, startColumn + 1, startRow);
                    return (object)entryEntry;
                case ComponentType.Picker:
                    var valuePicker = new Picker
                    {
                        // Цвета
                        TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                        // Текст
                        FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,

                        ItemsSource = items ?? new List<ItemPicker>(),
                        ItemDisplayBinding = new Binding("Name"),
                    };
                    if (idChangedAction != null)
                    {
                        valuePicker.SelectedIndexChanged += (sender, e) =>
                        {
                            if (valuePicker.SelectedItem is ItemPicker selectedItem)
                            {
                                idChangedAction(selectedItem.Id);
                            }
                        };
                    }

                    grid.Add(valuePicker, startColumn + 1, startRow);
                    return (object)valuePicker;
            }

            return new object();
        }
    }
}
