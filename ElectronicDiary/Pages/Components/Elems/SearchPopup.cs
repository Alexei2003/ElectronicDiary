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
                Padding = UserData.UserSettings.Sizes.PADDING_ALL_PAGES,
                Spacing = UserData.UserSettings.Sizes.SPACING_ALL_PAGES,

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
