﻿using CommunityToolkit.Maui.Views;

using ElectronicDiary.SaveData.Other;
using ElectronicDiary.SaveData.Static;
using ElectronicDiary.Web.DTO.Responses.Other;

namespace ElectronicDiary.Pages.Others
{
    public partial class SearchPopup : Popup
    {
        public List<TypeResponse> AllItems { get; set; }
        private readonly ListView _listView;

        public SearchPopup(List<TypeResponse> items, Action<long> IdChangedAction)
        {
            AllItems = items;

            var searchBar = new SearchBar
            {
                // Цвета
                BackgroundColor = UserData.Settings.Theme.AccentColorFields,
                TextColor = UserData.Settings.Theme.TextColor,
                PlaceholderColor = UserData.Settings.Theme.PlaceholderColor,

                // Текст
                FontSize = UserData.Settings.Fonts.BaseFontSize,
                Placeholder = "Введите для поиска...",
            };

            searchBar.TextChanged += (sender, e) =>
            {
                FilterItems(e.NewTextValue);
            };

            _listView = new ListView
            {
                // Цвета
                BackgroundColor = UserData.Settings.Theme.BackgroundPageColor,

                ItemsSource = AllItems,
                ItemTemplate = new DataTemplate(() =>
                {
                    var label = new Label();
                    label.BackgroundColor = UserData.Settings.Theme.AccentColorFields;
                    label.TextColor = UserData.Settings.Theme.TextColor;
                    label.SetBinding(Label.TextProperty, "Name");
                    label.Padding = UserData.Settings.Sizes.Padding;
                    return new ViewCell { View = label };
                })
            };

            _listView.ItemTapped += (sender, e) =>
            {
                if (e.Item is TypeResponse selectedItem)
                {
                    HandleItemTapped(selectedItem, IdChangedAction);
                }
            };

            var stackLayout = new StackLayout
            {
                // Положение
                MinimumWidthRequest = (Application.Current?.Windows[0].Width ?? 0),
                MinimumHeightRequest = (Application.Current?.Windows[0].Height ?? 0) * 0.90,

                Padding = UserData.Settings.Sizes.Padding,
                Spacing = UserData.Settings.Sizes.Spacing,

                // Цвета
                BackgroundColor = UserData.Settings.Theme.BackgroundPageColor,

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
                _listView.ItemsSource = AllItems.Where(item => (item.Name ?? string.Empty).Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
            }
        }

        private void HandleItemTapped(TypeResponse selectedItem, Action<long> IdChangedAction)
        {
            IdChangedAction(selectedItem.Id);
            CloseAsync();
        }
    }
}
