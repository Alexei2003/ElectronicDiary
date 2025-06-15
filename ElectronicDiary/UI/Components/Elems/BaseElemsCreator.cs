using System.Collections.ObjectModel;

using CommunityToolkit.Maui.Extensions;

using ElectronicDiary.Pages.Others;
using ElectronicDiary.SaveData.Static;
using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.UI.Components.Elems
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

                FontSize = UserData.Settings.Fonts.BaseFontSize,
                Text = text,
            };

            button.Clicked += handler;

            button.Pressed += (sender, e) =>
            {
                button.ScaleTo(0.97, 50, Easing.SinInOut);
                button.FadeTo(0.5, 50);
            };

            button.Released += (sender, e) =>
            {
                button.ScaleTo(1.0, 50, Easing.SinInOut);
                button.FadeTo(1.0, 50);
            };

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
                MaximumWidthRequest = UserData.Settings.Sizes.ImageButton,
                MaximumHeightRequest = UserData.Settings.Sizes.ImageButton,
                Source = ImageSource.FromFile(imagePath)
            };

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += handler;
            tapGesture.Tapped += (sender, e) =>
            {
                image.ScaleTo(0.97, 50, Easing.SinInOut);
                image.FadeTo(0.5, 50);
            };
            image.Unfocused += (sender, e) =>
            {
                image.ScaleTo(1.0, 50, Easing.SinInOut);
                image.FadeTo(1.0, 50);
            };
            image.GestureRecognizers.Add(tapGesture);

            return image;
        }

        public static VerticalStackLayout CreateImageFromUrl(string? url, EventHandler<TappedEventArgs>? handler)
        {
            var vStack = new VerticalStackLayout();

            var loadImage = new Image
            {
                MaximumWidthRequest = UserData.Settings.Sizes.Image,
                MaximumHeightRequest = UserData.Settings.Sizes.Image,
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
                    MaximumHeightRequest = UserData.Settings.Sizes.Image,
                    MinimumHeightRequest = UserData.Settings.Sizes.Image,
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

                FontSize = UserData.Settings.Fonts.BaseFontSize,
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

        public static Label CreateLabel(string? text, int maxLength = 100)
        {
            if (text != null && maxLength > 0 && text.Length > maxLength)
            {
                text = text.Substring(0, maxLength) + " …";
            }

            var label = new Label
            {
                TextColor = UserData.Settings.Theme.TextColor,

                FontSize = UserData.Settings.Fonts.BaseFontSize,
                Text = text ?? string.Empty,
            };

            return label;
        }

        public static Label CreateTitleLabel(string? text, int maxLength = -1)
        {
            var label = CreateLabel(text, maxLength);
            label.FontSize = 2 * UserData.Settings.Fonts.BaseFontSize;
            label.HorizontalTextAlignment = TextAlignment.Center;
            return label;
        }

        public static Label CreateSearchPopupAsLabel(List<TypeResponse> itemList, Action<long> idChangedAction)
        {
            var searchLabel = CreateLabel("Поиск");
            searchLabel.BackgroundColor = UserData.Settings.Theme.AccentColorFields;
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += (sender, e) =>
            {
                long id = -1;
                var popup = new SearchPopup(itemList, newText => id = newText);

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

        public static Picker CreatePicker(ObservableCollection<TypeResponse> itemList, Action<long> idChangedAction, long? baseSelectedId = null)
        {
            var picker = new Picker
            {
                BackgroundColor = UserData.Settings.Theme.AccentColorFields,
                TextColor = UserData.Settings.Theme.TextColor,

                FontSize = UserData.Settings.Fonts.BaseFontSize,

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
                    if (picker.SelectedItem is TypeResponse selectedItem)
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
                ColumnSpacing = UserData.Settings.Sizes.Spacing,
                RowSpacing = UserData.Settings.Sizes.Spacing,

                BackgroundColor = UserData.Settings.Theme.BackgroundFillColor,
            };
            if (padding) { grid.Padding = UserData.Settings.Sizes.Padding; }

            GridAddColumn(grid, countColumns);

            return grid;
        }

        public static void GridAddColumn(Grid grid, int countColumns, GridLength? width = null)
        {
            if (width == null)
            {
                width = GridLength.Star;
            }
            for (int i = 0; i < countColumns; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = width.Value });
            }
        }

        public static void GridRemoveColumn(Grid grid, int countColumns, GridLength? width = null)
        {
            if (width == null)
            {
                width = GridLength.Star;
            }
            for (int i = 0; i < countColumns; i++)
            {
                grid.ColumnDefinitions.Remove(grid.ColumnDefinitions.Last());
            }
        }

        public static VerticalStackLayout CreateVerticalStackLayout()
        {
            var verticalStackLayout = new VerticalStackLayout
            {
                // Положение
                HorizontalOptions = LayoutOptions.Fill,
                Padding = UserData.Settings.Sizes.Padding,
                Spacing = UserData.Settings.Sizes.Spacing,
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
