using CommunityToolkit.Maui.Views;

using ElectronicDiary.Pages.Others;
using ElectronicDiary.SaveData;

namespace ElectronicDiary.Pages.Components.Elems
{
    public static class BaseElemCreator
    {
        public static Button CreateButton(string text, EventHandler handler)
        {
            var button = new Button()
            {
                HorizontalOptions = LayoutOptions.Fill,

                BackgroundColor = UserData.UserSettings.Colors.ACCENT_COLOR,
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

        public static SearchPopup CreateSearchPopup(List<Item> items, Action<long> idChangedAction)
        {
            var popup = new SearchPopup(items, idChangedAction);

            var page = Application.Current?.Windows[0].Page;
            page?.ShowPopup(popup);

            return popup;
        }

        public static Picker CreatePicker(List<Item>? items, Action<long> idChangedAction, long? baseSelectedId)
        {
            var picker = new Picker
            {
                // Цвета
                TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                // Текст
                FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,

                ItemsSource = items ?? [],
                ItemDisplayBinding = new Binding("Name"),
            };

            if (items != null && baseSelectedId != null)
            {
                var selectedIndex = items.FindIndex(item => item.Id == baseSelectedId);
                picker.SelectedIndex = selectedIndex >= 0 ? selectedIndex : 0;
            }

            if (idChangedAction != null)
            {
                picker.SelectedIndexChanged += (sender, e) =>
                {
                    if (picker.SelectedItem is Item selectedItem && selectedItem.Id != null)
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
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Fill,
                Padding = UserData.UserSettings.Sizes.PADDING_ALL_PAGES,
                Spacing = UserData.UserSettings.Sizes.SPACING_ALL_PAGES,
            };

            return verticalStackLayout;
        }

    }
}
