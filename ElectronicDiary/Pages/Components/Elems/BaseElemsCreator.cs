using System.Collections.ObjectModel;

using CommunityToolkit.Maui.Views;

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

                BackgroundColor = navigation ? UserData.Settings.Theme.NavigationPageColor : UserData.Settings.Theme.AccentColor,
                TextColor = UserData.Settings.Theme.TextColor,

                FontSize = UserData.Settings.Fonts.BASE_FONT_SIZE,
                Text = text,
            };

            button.Clicked += handler;
            return button;
        }

        public static async Task<string> CreateActionSheet(string[] actionList)
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

        public static Image CreateImageFromFile(string? imagePath, EventHandler<TappedEventArgs> handler)
        {
            var image = new Image
            {
                MaximumWidthRequest = UserData.Settings.Sizes.IMAGE_BUTTON_SIZE,
                MaximumHeightRequest = UserData.Settings.Sizes.IMAGE_BUTTON_SIZE,
                Source = ImageSource.FromFile(imagePath)
            };

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += handler;
            image.GestureRecognizers.Add(tapGesture);

            return image;
        }

        public static VerticalStackLayout CreateImageFromUrl(string? url, EventHandler<TappedEventArgs>? handler)
        {
            var vStack = new VerticalStackLayout();

            var loadImage = new Image
            {
                MaximumWidthRequest = UserData.Settings.Sizes.IMAGE_SIZE,
                MaximumHeightRequest = UserData.Settings.Sizes.IMAGE_SIZE,
                Source = ImageSource.FromFile(url == null ? "no_image.png" : "loading_image.png")
            };
            if (handler != null)
            {
                var tapGesture = new TapGestureRecognizer();
                tapGesture.Tapped += handler;
                loadImage.GestureRecognizers.Add(tapGesture);
            }

            vStack.Add(loadImage);

            if (url != null)
            {
                var mainImage = new Image
                {
                    MaximumHeightRequest = UserData.Settings.Sizes.IMAGE_SIZE,
                    MinimumHeightRequest = UserData.Settings.Sizes.IMAGE_SIZE,
                    Source = ImageSource.FromUri(new Uri(url + $"?t={DateTime.Now.Ticks}")),
                };
                if (handler != null)
                {
                    var tapGesture = new TapGestureRecognizer();
                    tapGesture.Tapped += handler;
                    mainImage.GestureRecognizers.Add(tapGesture);
                }

                vStack.Add(mainImage);

                loadImage.IsVisible = true;
                mainImage.IsVisible = false;
                Task.Run(async () =>
                {
                    while (!mainImage.IsVisible)
                    {
                        await Task.Delay(1000);
                        Application.Current?.Dispatcher.Dispatch(() =>
                        {
                            loadImage.IsVisible = mainImage.IsLoading;
                            mainImage.IsVisible = !mainImage.IsLoading;
                        });
                    }
                });
            }

            return vStack;
        }

        public static Editor CreateEditor(Action<string>? textChangedAction, string? placeholder, string? text = null)
        {
            var editor = new Editor
            {
                BackgroundColor = UserData.Settings.Theme.AccentColorFields,
                TextColor = UserData.Settings.Theme.TextColor,
                PlaceholderColor = UserData.Settings.Theme.PlaceholderColor,

                FontSize = UserData.Settings.Fonts.BASE_FONT_SIZE,
                Placeholder = placeholder ?? string.Empty,
                Text = text ?? string.Empty,

                AutoSize = EditorAutoSizeOption.TextChanges,
            };
            if (textChangedAction != null)
            {
                editor.TextChanged += (sender, e) => textChangedAction(e.NewTextValue);
            }

            return editor;
        }

        public static Label CreateLabel(string? text, int maxLength = -1)
        {
            if (text != null && maxLength > 0 && text.Length > maxLength)
            {
                text = text.Substring(0, maxLength) + " …";
            }

            var label = new Label
            {
                TextColor = UserData.Settings.Theme.TextColor,

                FontSize = UserData.Settings.Fonts.BASE_FONT_SIZE,
                Text = text ?? string.Empty,
            };

            return label;
        }

        public static Label CreateSearchPopupAsLabel(List<Item> itemList, Action<long> idChangedAction)
        {
            var searchLabel = CreateLabel("Поиск");
            searchLabel.BackgroundColor = UserData.Settings.Theme.AccentColorFields;
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
                BackgroundColor = UserData.Settings.Theme.AccentColorFields,
                TextColor = UserData.Settings.Theme.TextColor,

                FontSize = UserData.Settings.Fonts.BASE_FONT_SIZE,

                ItemsSource = itemList,
                ItemDisplayBinding = new Binding("Name"),
            };

            if (itemList != null && baseSelectedId != null)
            {
                var item = itemList.FirstOrDefault(i => i.Id == baseSelectedId);
                if (item != null)
                {
                    var selectedIndex = itemList.IndexOf(item);
                    picker.SelectedIndex = selectedIndex >= 0 ? selectedIndex : 0;
                }
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



        public static Grid CreateGrid(int countColumns = 2, bool padding = true)
        {
            var grid = new Grid
            {
                ColumnSpacing = UserData.Settings.Sizes.SPACING_ALL_PAGES,
                RowSpacing = UserData.Settings.Sizes.SPACING_ALL_PAGES,

                BackgroundColor = UserData.Settings.Theme.BackgroundFillColor,
            };
            if (padding) { grid.Padding = UserData.Settings.Sizes.PADDING_ALL_PAGES; }

            for (int i = 0; i < countColumns; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            }

            return grid;
        }

        public static VerticalStackLayout CreateVerticalStackLayout()
        {
            var verticalStackLayout = new VerticalStackLayout
            {
                // Положение
                HorizontalOptions = LayoutOptions.Fill,
                Padding = UserData.Settings.Sizes.PADDING_ALL_PAGES,
                Spacing = UserData.Settings.Sizes.SPACING_ALL_PAGES,
            };

            return verticalStackLayout;
        }

        public static HorizontalStackLayout CreateHorizontalStackLayout()
        {
            var hStack = new HorizontalStackLayout()
            {
                // Положение
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Fill,
            };

            return hStack;
        }

    }
}
