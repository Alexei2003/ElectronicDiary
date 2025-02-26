using ElectronicDiary.Pages.Otherts;
using ElectronicDiary.SaveData;
using ElectronicDiary.Web.Api;
using ElectronicDiary.Web.DTO;
using Microsoft.Maui.Controls.Shapes;
using System.Text.Json;

namespace ElectronicDiary.Pages
{
    public class AdminPage : ContentPage
    {
        private readonly HorizontalStackLayout _mainStack;
        private readonly List<VerticalStackLayout> _stacksList = [];

        public AdminPage()
        {
            Title = "Панель администратора";
            ToolbarItemsAdder.AddSettings(ToolbarItems);
            ToolbarItemsAdder.AddLogOut(ToolbarItems);

            // Цвета
            BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_PAGE_COLOR;

            _stacksList.Add(CreateEducationalInstitutionsStack());

            _mainStack = new HorizontalStackLayout
            {
                // Положение
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Fill,
            };

            Content = _mainStack;
            SizeChanged += WindowSizeChanged;

            GetEducationalInstitutionList();
        }

        private async void WindowSizeChanged(object? sender, EventArgs e)
        {
            _mainStack.Clear();

            double dpi = DeviceDisplay.MainDisplayInfo.Density * 160;
            var countColumn = int.Min((int)(Width / dpi / 2), 3);

            for (var i = int.Max(_stacksList.Count - countColumn, 0); i < _stacksList.Count; i++)
            {
                _mainStack.Add(_stacksList[i]);
            }
        }



        private void AddLineEntry(Grid grid, int startColumn, int startRow, string title, string placeholder, Action<string> textChangedAction)
        {
            var label = new Label
            {
                // Цвета
                TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                // Текст
                FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                Text = title,
            };

            var entry = new Entry
            {
                // Цвета
                BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_FILL_COLOR,
                TextColor = UserData.UserSettings.Colors.TEXT_COLOR,
                PlaceholderColor = UserData.UserSettings.Colors.PLACEHOLDER_COLOR,

                // Текст
                FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                Placeholder = placeholder
            };
            entry.TextChanged += (sender, e) => textChangedAction(e.NewTextValue);

            grid.Add(label, startColumn, startRow);
            grid.Add(entry, startColumn + 1, startRow);
        }

        private void AddLineLabel(Grid grid, int startColumn, int startRow, string title, string value)
        {
            var titleLabel = new Label
            {
                // Цвета
                TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                // Текст
                FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                Text = title,
            };

            var valueLabel = new Label
            {
                // Цвета
                TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                // Текст
                FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                Text = value,
            };

            grid.Add(titleLabel, startColumn, startRow);
            grid.Add(valueLabel, startColumn + 1, startRow);
        }



        private string _educationalInstitutionRegion = "";
        private string _educationalInstitutionSettlement = "";
        private string _educationalInstitutionName = "";
        private VerticalStackLayout _educationalInstitutionVerticalStackLayout;
        private VerticalStackLayout CreateEducationalInstitutionsStack()
        {
            var grid = new Grid
            {
                // Положение
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Star }
                },
                Padding = PageConstants.PADDING_ALL_PAGES,
                ColumnSpacing = PageConstants.SPACING_ALL_PAGES,
                RowSpacing = PageConstants.SPACING_ALL_PAGES,

                // Цвета
                BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_FILL_COLOR,
            };

            AddLineEntry(
                grid: grid,
                startColumn: 0,
                startRow: 0,
                title: "Область",
                placeholder: "Минская область",
                textChangedAction: newText => _educationalInstitutionRegion = newText
            );

            AddLineEntry(
                grid: grid,
                startColumn: 0,
                startRow: 1,
                title: "Населённый пункт",
                placeholder: "Солигорск",
                textChangedAction: newText => _educationalInstitutionSettlement = newText
            );

            AddLineEntry(
                grid: grid,
                startColumn: 0,
                startRow: 2,
                title: "Название",
                placeholder: "ГУО ...",
                textChangedAction: newText => _educationalInstitutionName = newText
            );

            var verticalStack = new VerticalStackLayout
            {
                // Положение
                HorizontalOptions = LayoutOptions.Start,
                Padding = PageConstants.PADDING_ALL_PAGES,
                Spacing = PageConstants.SPACING_ALL_PAGES,
            };

            var GetEducationalInstitutionButton = new Button
            {
                // Положение
                HorizontalOptions = LayoutOptions.Center,

                // Цвета
                BackgroundColor = UserData.UserSettings.Colors.ACCENT_COLOR,
                TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                // Текст
                FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                Text = "Найти",
            };
            GetEducationalInstitutionButton.Clicked += GetEducationalInstitutionButtonClicked;
            grid.Add(GetEducationalInstitutionButton, 0, 3);
            verticalStack.Add(grid);

            _educationalInstitutionVerticalStackLayout = new VerticalStackLayout()
            {
                Spacing = PageConstants.SPACING_ALL_PAGES
            };
            verticalStack.Add(_educationalInstitutionVerticalStackLayout);

            return verticalStack;
        }

        private async Task GetEducationalInstitutionList()
        {
            var response = await Admin.GetSchools();
            if (response.Error)
            {
                await DisplayAlert("Ошибка", response.Message, "OK");
            }
            else
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var educationalInstitutionDTOList = JsonSerializer.Deserialize<List<EducationalInstitutionDTO>>(response.Message, options);
                educationalInstitutionDTOList = educationalInstitutionDTOList
                    .Where(e =>
                        (_educationalInstitutionRegion == "" || e.Settlement.Region.Name.Contains(_educationalInstitutionRegion, StringComparison.OrdinalIgnoreCase)) &&
                        (_educationalInstitutionSettlement == "" || e.Settlement.Name.Contains(_educationalInstitutionSettlement, StringComparison.OrdinalIgnoreCase)) &&
                        (_educationalInstitutionName == "" || e.Name.Contains(_educationalInstitutionName, StringComparison.OrdinalIgnoreCase)))
                    .ToList();

                _educationalInstitutionVerticalStackLayout.Clear();

                for (var i = 0; i < educationalInstitutionDTOList.Count; i++)
                {
                    var tapGesture = new TapGestureRecognizer();
                    tapGesture.Tapped += GetEducationalInstitutionGestureTapped;
                    var grid = new Grid
                    {
                        // Положение
                        ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = GridLength.Auto },
                            new ColumnDefinition { Width = GridLength.Star }
                        },
                        Padding = PageConstants.PADDING_ALL_PAGES,
                        ColumnSpacing = PageConstants.SPACING_ALL_PAGES,
                        RowSpacing = PageConstants.SPACING_ALL_PAGES,

                        // Цвета
                        BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_FILL_COLOR,
                    };
                    grid.GestureRecognizers.Add(tapGesture);

                    AddLineLabel(
                        grid: grid,
                        startColumn: 0,
                        startRow: 0,
                        title: "Название",
                        value: educationalInstitutionDTOList[i].Name
                    );

                    AddLineLabel(
                        grid: grid,
                        startColumn: 0,
                        startRow: 1,
                        title: "Регион",
                        value: educationalInstitutionDTOList[i].Settlement.Region.Name
                    );

                    AddLineLabel(
                        grid: grid,
                        startColumn: 0,
                        startRow: 2,
                        title: "Город",
                        value: educationalInstitutionDTOList[i].Settlement.Name
                    );

                    _educationalInstitutionVerticalStackLayout.Add(grid);
                }
            }
        }
        private async void GetEducationalInstitutionButtonClicked(object? sender, EventArgs e)
        {
            await GetEducationalInstitutionList();
        }
        private async void GetEducationalInstitutionGestureTapped(object? sender, EventArgs e)
        {

        }




    }
}
