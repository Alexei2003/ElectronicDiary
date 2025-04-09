using System.Collections.ObjectModel;
using System.Security.Cryptography;

using CommunityToolkit.Maui.Views;

using ElectronicDiary.Pages.Components.Other;
using ElectronicDiary.Pages.Others;
using ElectronicDiary.SaveData.Static;

namespace ElectronicDiary.Pages.Components.Elems
{
    public static class BaseElemsCreator
    {
        public static Button CreateButton(string text, EventHandler handler, bool navigation = false)
        {
            var button = new Button()
            {
                HorizontalOptions = LayoutOptions.Fill,

                BackgroundColor = navigation ? UserData.UserSettings.Colors.NAVIGATION_PAGE_COLOR : UserData.UserSettings.Colors.ACCENT_COLOR,
                TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                Text = text,
            };

            button.Clicked += handler;
            return button;
        }

        public static async Task<string> CreateActionSheetCreator(string[] actionList)
        {
            var page = Application.Current?.Windows[0].Page;
            var action = string.Empty;
            if (page != null) action = await page.DisplayActionSheet(
                "Выберите действие",
                "Отмена",
                null,
                actionList);

            return action;
        }

        public static Image CreateImage(string? url)
        {
            var tmpUrl = url ?? PageConstants.NO_IMAGE_URL;
            var image = new Image
            {
                MaximumWidthRequest = UserData.UserSettings.Sizes.IMAGE_SIZE,
                MaximumHeightRequest = UserData.UserSettings.Sizes.IMAGE_SIZE,
                Source = ImageSource.FromUri(new Uri(tmpUrl))
            };

            return image;
        }

        public static Entry CreateEntry(Action<string>? textChangedAction, string? placeholder, string? text = null)
        {
            var entry = new Entry
            {
                BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_FILL_COLOR,
                TextColor = UserData.UserSettings.Colors.TEXT_COLOR,
                PlaceholderColor = UserData.UserSettings.Colors.PLACEHOLDER_COLOR,

                FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                Placeholder = placeholder ?? string.Empty,
                Text = text ?? string.Empty,
            };
            if (textChangedAction != null)
            {
                entry.TextChanged += (sender, e) => textChangedAction(e.NewTextValue);
            }

            return entry;
        }

        public static Label CreateLabel(string? text)
        {
            var label = new Label
            {
                TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                Text = text ?? string.Empty,
            };

            return label;
        }

        public static Label CreateSearchPopupAsLabel(List<Item> itemList, Action<long> idChangedAction)
        {
            var searchLabel = CreateLabel("Поиск");
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += (sender, e) =>
            {
                long id = -1;
                Action<long> idChangedActionLocal = newText => id = newText;

                var popup = new SearchPopup(itemList, idChangedActionLocal);

                popup.Closed += (sender, e) =>
                {
                    if (popup.AllItems.Count > 0 && id > -1)
                    {
                        if (idChangedAction != null) idChangedAction(id);
                        searchLabel.Text = popup.AllItems.FirstOrDefault(item => item.Id == id)?.Name ?? "Найти";
                    }
                    searchLabel.Focus();
                };

                var page = Application.Current?.Windows[0].Page;
                page?.ShowPopup(popup);
            };

            searchLabel.GestureRecognizers.Add(tapGesture);
            return searchLabel;
        }

        public static Picker CreatePicker(ObservableCollection<Item> itemList, Action<long> idChangedAction, long? baseSelectedId = null)
        {
            var picker = new Picker
            {
                // Цвета
                TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                // Текст
                FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,

                ItemsSource = itemList,
                ItemDisplayBinding = new Binding("Name"),
            };

            if (itemList != null && baseSelectedId != null)
            {
                var selectedIndex = itemList.IndexOf(itemList.FirstOrDefault(i => i.Id == baseSelectedId));
                picker.SelectedIndex = selectedIndex >= 0 ? selectedIndex : 0;
            }

            if (idChangedAction != null)
            {
                picker.SelectedIndexChanged += (sender, e) =>
                {
                    if (picker.SelectedItem is Item selectedItem)
                    {
                        idChangedAction(selectedItem.Id);
                    }
                };
            }

            return picker;
        }



        public static Grid CreateGrid()
        {
            var grid = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star }
                },

                Padding = UserData.UserSettings.Sizes.PADDING_ALL_PAGES,
                ColumnSpacing = UserData.UserSettings.Sizes.SPACING_ALL_PAGES,
                RowSpacing = UserData.UserSettings.Sizes.SPACING_ALL_PAGES,

                BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_FILL_COLOR,
            };

            return grid;
        }

        public static VerticalStackLayout CreateVerticalStackLayout()
        {
            var verticalStackLayout = new VerticalStackLayout
            {
                // Положение
                HorizontalOptions = LayoutOptions.Fill,
                Padding = UserData.UserSettings.Sizes.PADDING_ALL_PAGES,
                Spacing = UserData.UserSettings.Sizes.SPACING_ALL_PAGES,
            };

            return verticalStackLayout;
        }

    }
}
