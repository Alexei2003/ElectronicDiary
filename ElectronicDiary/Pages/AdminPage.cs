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
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public AdminPage()
        {
            Title = "Панель администратора";
            ToolbarItemsAdder.AddSettings(ToolbarItems);
            ToolbarItemsAdder.AddLogOut(ToolbarItems);

            // Цвета
            BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_PAGE_COLOR;

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            _stacksList.Add(CreateEducationalInstitutionListStack());

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

        private void RepaintPage()
        {
            _mainStack.Clear();

            double dpi = DeviceDisplay.MainDisplayInfo.Density * 160;
            var countColumn = int.Min((int)(Width / dpi / 2), 3);

            for (var i = int.Max(_stacksList.Count - countColumn, 0); i < _stacksList.Count; i++)
            {
                _mainStack.Add(_stacksList[i]);
            }
        }

        private async void WindowSizeChanged(object? sender, EventArgs e)
        {
            RepaintPage();
        }

        protected override bool OnBackButtonPressed()
        {
            if(_stacksList.Count > 1)
            {
                _stacksList.RemoveAt(_stacksList.Count-1);
                RepaintPage();
            }

            return true;
        }



        private enum PageType 
        {
            EducationalInstitutionList, EducationalInstitution
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

        private void AddLineLabel(Grid grid, int startColumn, int startRow, string title, string? value)
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
                Text = value ?? "",
            };

            grid.Add(titleLabel, startColumn, startRow);
            grid.Add(valueLabel, startColumn + 1, startRow);
        }



        private string _educationalInstitutionRegion = "";
        private string _educationalInstitutionSettlement = "";
        private string _educationalInstitutionName = "";
        private VerticalStackLayout _educationalInstitutionVerticalStackLayout;
        private VerticalStackLayout CreateEducationalInstitutionListStack()
        {
            var verticalStack = new VerticalStackLayout
            {
                // Положение
                HorizontalOptions = LayoutOptions.Start,
                Padding = PageConstants.PADDING_ALL_PAGES,
                Spacing = PageConstants.SPACING_ALL_PAGES,
            };
            verticalStack.BindingContext = PageType.EducationalInstitutionList;

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

            var rowIndex = 0;

            AddLineEntry(
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Область",
                placeholder: "Минская область",
                textChangedAction: newText => _educationalInstitutionRegion = newText
            );

            AddLineEntry(
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Населённый пункт",
                placeholder: "Солигорск",
                textChangedAction: newText => _educationalInstitutionSettlement = newText
            );

            AddLineEntry(
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Название",
                placeholder: "ГУО ...",
                textChangedAction: newText => _educationalInstitutionName = newText
            );

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
            GetEducationalInstitutionButton.Clicked += GetEducationalInstitutionListButtonClicked;
            grid.Add(GetEducationalInstitutionButton, 0, 3);
            verticalStack.Add(grid);

            var addEducationalInstitutionButton = new Button
            {
                // Положение
                HorizontalOptions = LayoutOptions.Center,

                // Цвета
                BackgroundColor = UserData.UserSettings.Colors.ACCENT_COLOR,
                TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                // Текст
                FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                Text = "Добавить",
            };
            addEducationalInstitutionButton.Clicked += AddEducationalInstitutionButtonClicked;
            verticalStack.Add(addEducationalInstitutionButton);

            _educationalInstitutionVerticalStackLayout = new VerticalStackLayout()
            {
                Spacing = PageConstants.SPACING_ALL_PAGES
            };
            verticalStack.Add(_educationalInstitutionVerticalStackLayout);

            return verticalStack;
        }
        private async void AddEducationalInstitutionButtonClicked(object? sender, EventArgs e)
        {
            
        }

        private List<EducationalInstitutionDTO> _educationalInstitutionDTOList = new();
        private async Task GetEducationalInstitutionList()
        {
            var response = await Admin.GetSchools();
            if (response.Error)
            {
                await DisplayAlert("Ошибка", response.Message, "OK");
            }
            else
            {
                _educationalInstitutionDTOList = JsonSerializer.Deserialize<List<EducationalInstitutionDTO>>(response.Message, _jsonSerializerOptions);
                _educationalInstitutionDTOList = _educationalInstitutionDTOList
                    .Where(e =>
                        (_educationalInstitutionRegion == "" || e.Settlement.Region.Name.Contains(_educationalInstitutionRegion, StringComparison.OrdinalIgnoreCase)) &&
                        (_educationalInstitutionSettlement == "" || e.Settlement.Name.Contains(_educationalInstitutionSettlement, StringComparison.OrdinalIgnoreCase)) &&
                        (_educationalInstitutionName == "" || e.Name.Contains(_educationalInstitutionName, StringComparison.OrdinalIgnoreCase)))
                    .ToList();

                _educationalInstitutionVerticalStackLayout.Clear();

                for (var i = 0; i < _educationalInstitutionDTOList.Count; i++)
                {
                    var tapGesture = new TapGestureRecognizer();
                    tapGesture.Tapped += GetEducationalInstitutionListGestureTapped;
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
                    grid.BindingContext = _educationalInstitutionDTOList[i].Id;
                    grid.GestureRecognizers.Add(tapGesture);

                    var rowIndex = 0;

                    AddLineLabel(
                        grid: grid,
                        startColumn: 0,
                        startRow: rowIndex++,
                        title: "Название",
                        value: _educationalInstitutionDTOList[i].Name
                    );

                    AddLineLabel(
                        grid: grid,
                        startColumn: 0,
                        startRow: rowIndex++,
                        title: "Регион",
                        value: _educationalInstitutionDTOList[i].Settlement.Region.Name
                    );

                    AddLineLabel(
                        grid: grid,
                        startColumn: 0,
                        startRow: rowIndex++,
                        title: "Город",
                        value: _educationalInstitutionDTOList[i].Settlement.Name
                    );

                    _educationalInstitutionVerticalStackLayout.Add(grid);
                }
            }
        }
        private async void GetEducationalInstitutionListButtonClicked(object? sender, EventArgs e)
        {
            await GetEducationalInstitutionList();
        }
        private async void GetEducationalInstitutionListGestureTapped(object? sender, EventArgs e)
        {
            string action = await Application.Current.MainPage.DisplayActionSheet(
                "Выберите действие",    // Заголовок
                "Отмена",               // Кнопка отмены
                null,                   // Кнопка деструктивного действия (например, удаление)
                "Описание",             // Остальные кнопки
                "Перейти",
                "Редактировать",
                "Удалить");

            var id = (int)((Grid)sender).BindingContext;


            VerticalStackLayout verticalStack;
            switch (action)
            {
                case "Описание":
                    verticalStack = CreateEducationalInstitutionStack(id);
                    if ((PageType)_stacksList[^1].BindingContext == PageType.EducationalInstitution)
                    {
                        _stacksList.RemoveAt(_stacksList.Count - 1);
                    }
                    _stacksList.Add(verticalStack);
                    break;
                case "Перейти":

                    break;
                case "Редактировать":

                    break;
                case "Удалить":

                    break;
                default:
                    return;
            }

            await GetEducationalInstitutionList();
            RepaintPage();
        }

        private VerticalStackLayout CreateEducationalInstitutionStack(int id)
        {
            var verticalStack = new VerticalStackLayout
            {
                // Положение
                HorizontalOptions = LayoutOptions.Start,
                Padding = PageConstants.PADDING_ALL_PAGES,
                Spacing = PageConstants.SPACING_ALL_PAGES,
            };
            verticalStack.BindingContext = PageType.EducationalInstitution;

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

            var educationalInstitution = _educationalInstitutionDTOList.FirstOrDefault(x => x.Id == id);

            var rowIndex = 0;

            AddLineLabel(
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Название",
                value: educationalInstitution.Name
            );

            AddLineLabel(
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Регион",
                value: educationalInstitution.Settlement.Region.Name
            );

            AddLineLabel(
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Населённый пункт",
                value: educationalInstitution.Settlement.Name
            );

            AddLineLabel(
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Адресс",
                value: educationalInstitution.Address
            );

            AddLineLabel(
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Email",
                value: educationalInstitution.Email
            );

            AddLineLabel(
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Телефон",
                value: educationalInstitution.PhoneNumber
            );

            verticalStack.Add(grid);
            return verticalStack;
        }
    }
}
