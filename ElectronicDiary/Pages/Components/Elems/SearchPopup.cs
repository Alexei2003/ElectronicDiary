using CommunityToolkit.Maui.Views;

using ElectronicDiary.Pages.Components.Elems;
using ElectronicDiary.SaveData.Static;

namespace ElectronicDiary.Pages.Others
{
    public partial class SearchPopup : Popup
    {
        public List<Item> AllItems { get; set; }
        private readonly ListView _listView;

        public SearchPopup(List<Item> items, Action<long> IdChangedAction)
        {
            AllItems = items;

            Color = UserData.Settings.Theme.BackgroundPageColor;

            var searchBar = new SearchBar
            {
                // Цвета
                BackgroundColor = UserData.Settings.Theme.AccentColorFields,
                TextColor = UserData.Settings.Theme.TextColor,
                PlaceholderColor = UserData.Settings.Theme.PlaceholderColor,

                // Текст
                FontSize = UserData.Settings.Fonts.BASE_FONT_SIZE,
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
                    return new ViewCell { View = label };
                })
            };

            _listView.ItemTapped += (sender, e) =>
            {
                if (e.Item is Item selectedItem)
                {
                    HandleItemTapped(selectedItem, IdChangedAction);
                }
            };

            var stackLayout = new StackLayout
            {
                // Положение
                WidthRequest = (Application.Current?.Windows[0].Width ?? 0) / 1 * 0.90,
#if WINDOWS
                HeightRequest = 100,
#endif
                Padding = UserData.Settings.Sizes.PADDING_ALL_PAGES,
                Spacing = UserData.Settings.Sizes.SPACING_ALL_PAGES,

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

        private void HandleItemTapped(Item selectedItem, Action<long> IdChangedAction)
        {
            IdChangedAction(selectedItem.Id);
            Close();
        }
    }
}
