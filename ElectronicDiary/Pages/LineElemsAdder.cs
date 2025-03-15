using CommunityToolkit.Maui.Views;
using ElectronicDiary.SaveData;

namespace ElectronicDiary.Pages
{
    public static class LineElemsAdder
    {
        public sealed class ItemPicker
        {
            public int Id { get; set; } = -1;
            public string Name { get; set; } = "";
        }
        public class LabelData
        {
            public string? Title { get; set; } = null;
        }

        public class EntryData
        {
            public string? BaseText { get; set; } = null;
            public string? Placeholder { get; set; } = null;
            public Action<string>? TextChangedAction { get; set; } = null;
        }

        public class SearchData
        {
            public ItemPicker? BaseItem { get; set; } = null;
            public Action<long>? IdChangedAction { get; set; } = null;
        }

        public class PickerData
        {
            public long? BaseSelectedId { get; set; } = null;
            public List<ItemPicker>? Items { get; set; } = null;
            public Action<int>? IdChangedAction { get; set; } = null;
        }

        public static object[] AddLineElems(Grid grid, int rowIndex, object[] objectList)
        {
            var resultList = new List<object>();

            var indexColumn = 0;

            foreach (var obj in objectList)
            {
                switch (obj)
                {
                    case LabelData labelData:
                        var label = new Label
                        {
                            // Цвета
                            TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                            // Текст
                            FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                            Text = labelData.Title ?? "",
                        };

                        grid.Add(label, indexColumn++, rowIndex);
                        resultList.Add(label);
                        break;

                    case EntryData entryData:
                        var entry = new Entry
                        {
                            // Цвета
                            BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_FILL_COLOR,
                            TextColor = UserData.UserSettings.Colors.TEXT_COLOR,
                            PlaceholderColor = UserData.UserSettings.Colors.PLACEHOLDER_COLOR,

                            // Текст
                            FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                            Placeholder = entryData.Placeholder ?? "",
                            Text = entryData.BaseText ?? "",

                        };
                        if (entryData.TextChangedAction != null)
                        {
                            entry.TextChanged += (sender, e) => entryData.TextChangedAction(e.NewTextValue);
                        }

                        grid.Add(entry, indexColumn++, rowIndex);
                        resultList.Add(entry);
                        break;

                    case SearchData searchData:
                        var searchLabel = new Label
                        {
                            // Цвета
                            TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                            // Текст
                            FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                            Text = searchData?.BaseItem?.Name ?? "Найти",
                        };

                        long id = searchData?.BaseItem?.Id ?? 0;
                        if (searchData?.IdChangedAction != null)
                        {
                            searchData.IdChangedAction(id);
                        }
                        Action<long> idChangedActionLocal = newText => id = newText;

                        var tapGesture = new TapGestureRecognizer();
                        tapGesture.Tapped += (sender, e) =>
                        {
                            if (searchData?.IdChangedAction == null)
                            {
                                return;
                            }

                            var popup = new SearchPopup(tapGesture.BindingContext as List<ItemPicker>, idChangedActionLocal);
                            popup.Closed += (sender, e) =>
                            {
                                if (popup.AllItems.Count > 0 && id >= 0)
                                {
                                    searchData.IdChangedAction(id);
                                    searchLabel.Text = popup?.AllItems?.FirstOrDefault(item => item.Id == id)?.Name ?? "Найти";
                                }
                                searchLabel.Focus();
                            };
                            Application.Current.Windows[0].Page.ShowPopup(popup);
                        };
                        searchLabel.GestureRecognizers.Add(tapGesture);

                        grid.Add(searchLabel, indexColumn++, rowIndex);
                        resultList.Add(tapGesture);
                        break;

                    case PickerData pickerData:
                        var picker = new Picker
                        {
                            // Цвета
                            TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                            // Текст
                            FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,

                            ItemsSource = pickerData.Items ?? [],
                            ItemDisplayBinding = new Binding("Name"),
                        };
                        if (pickerData.Items != null && pickerData.BaseSelectedId != null)
                        {
                            var selectedIndex = pickerData.Items.FindIndex(item => item.Id == pickerData.BaseSelectedId);
                            picker.SelectedIndex = selectedIndex >= 0 ? selectedIndex : 0;
                        }


                        if (pickerData.IdChangedAction != null)
                        {
                            picker.SelectedIndexChanged += (sender, e) =>
                            {
                                if (picker.SelectedItem is ItemPicker selectedItem)
                                {
                                    pickerData.IdChangedAction(selectedItem.Id);
                                }
                            };
                        }

                        grid.Add(picker, indexColumn++, rowIndex);
                        resultList.Add(picker);
                        break;
                }
            }

            return resultList.ToArray();
        }
    }
}