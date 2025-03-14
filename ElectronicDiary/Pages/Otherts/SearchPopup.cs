﻿using CommunityToolkit.Maui.Views;
using ElectronicDiary.Pages;
using ElectronicDiary.Pages.Otherts;
using ElectronicDiary.SaveData;

namespace ElectronicDiary
{
    public class SearchPopup : Popup
    {
        public List<LineElemsAdder.ItemPicker> AllItems { get; set; }
        private ListView _listView;

        public SearchPopup(List<LineElemsAdder.ItemPicker>? items, Action<long> IdChangedAction)
        {
            AllItems = items ?? [];

            Color = UserData.UserSettings.Colors.BACKGROUND_FILL_COLOR;

            var searchBar = new SearchBar
            {
                // Цвета
                BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_FILL_COLOR,
                TextColor = UserData.UserSettings.Colors.TEXT_COLOR,
                PlaceholderColor = UserData.UserSettings.Colors.PLACEHOLDER_COLOR,

                // Текст
                FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                Placeholder = "Введите для поиска...",
            };

            searchBar.TextChanged += (sender, e) =>
            {
                FilterItems(e.NewTextValue);
            };

            _listView = new ListView
            {
                // Цвета
                BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_FILL_COLOR,

                ItemsSource = AllItems,
                ItemTemplate = new DataTemplate(() =>
                {
                    var label = new Label();
                    label.SetBinding(Label.TextProperty, "Name");
                    return new ViewCell { View = label };
                })
            };

            _listView.ItemTapped += (sender, e) =>
            {
                if (e.Item is LineElemsAdder.ItemPicker selectedItem)
                {
                    HandleItemTapped(selectedItem, IdChangedAction);
                }
            };

            var stackLayout = new StackLayout
            {
                // Положение
                WidthRequest = Application.Current.Windows[0].Width / 1 * 0.90,
#if WINDOWS
                HeightRequest = 100,
#endif
                Padding = PageConstants.PADDING_ALL_PAGES,
                Spacing = PageConstants.SPACING_ALL_PAGES,

                // Цвета
                BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_FILL_COLOR,

                Children =
                {
                    searchBar,
                    _listView,
                }
            };

            Content = stackLayout;
        }

        private void FilterItems(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                _listView.ItemsSource = AllItems;
            }
            else
            {
                _listView.ItemsSource = AllItems.Where(item => item.Name.ToLower().Contains(searchText.ToLower())).ToList();
            }
        }

        private void HandleItemTapped(LineElemsAdder.ItemPicker selectedItem, Action<long> IdChangedAction)
        {
            IdChangedAction(selectedItem.Id);
            Close();
        }
    }
}
