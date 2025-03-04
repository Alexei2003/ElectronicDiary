using ElectronicDiary.Pages.Otherts;
using ElectronicDiary.SaveData;
using ElectronicDiary.Web.Api.Educations;
using ElectronicDiary.Web.DTO;
using System.Text.Json;

namespace ElectronicDiary.Pages
{
    public class AdminPage : ContentPage
    {
        private readonly HorizontalStackLayout _mainStack;
        private readonly List<ScrollView> _viewList = [];
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

            _viewList.Add(CreateEducationalInstitutionListView());

            _mainStack = new HorizontalStackLayout
            {
                // Положение
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Fill,
            };

            RepaintPage();

            Content = _mainStack;
            SizeChanged += WindowSizeChanged;

            var task = GetEducationalInstitutionList();
            task.Wait();
        }

        // Обновление видов
        private void RepaintPage()
        {
            _mainStack.Clear();

            double dpi = DeviceDisplay.MainDisplayInfo.Density * 160;

            var countColumn = int.Min((int)(DeviceDisplay.MainDisplayInfo.Width / dpi / 2), 3);

            for (var i = int.Max(_viewList.Count - countColumn, 0); i < _viewList.Count; i++)
            {
                _viewList[i].WidthRequest = DeviceDisplay.MainDisplayInfo.Width / countColumn * 0.5;
                _mainStack.Add(_viewList[i]);
            }
        }

        private void WindowSizeChanged(object? sender, EventArgs e)
        {
            RepaintPage();
        }

        // Переопределение возврата
        protected override bool OnBackButtonPressed()
        {
            if (_viewList.Count > 1)
            {
                _viewList.RemoveAt(_viewList.Count - 1);
                RepaintPage();
            }

            return true;
        }



        // Тип вида
        private enum ViewType
        {
            EducationalInstitutionList, EducationalInstitution
        }



        // Добавление линий элементов в таблицах
        private static void AddLineElems(bool readOnly, Grid grid, int startColumn, int startRow, string title, string? value = null, string? placeholder = null, Action<string>? textChangedAction = null)
        {
            var titleLabel = new Label
            {
                // Цвета
                TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                // Текст
                FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                Text = title,
            };
            grid.Add(titleLabel, startColumn, startRow);

            if (readOnly)
            {
                var valueLabel = new Label
                {
                    // Цвета
                    TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                    // Текст
                    FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                    Text = value ?? "",
                };

                grid.Add(valueLabel, startColumn + 1, startRow);
            }
            else
            {
                var entryEntry = new Entry
                {
                    // Цвета
                    BackgroundColor = UserData.UserSettings.Colors.BACKGROUND_FILL_COLOR,
                    TextColor = UserData.UserSettings.Colors.TEXT_COLOR,
                    PlaceholderColor = UserData.UserSettings.Colors.PLACEHOLDER_COLOR,

                    // Текст
                    FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                    Placeholder = placeholder ?? ""
                };
                if (textChangedAction != null)
                {
                    entryEntry.TextChanged += (sender, e) => textChangedAction(e.NewTextValue);
                }

                grid.Add(entryEntry, startColumn + 1, startRow);
            }
        }


        // Список школ
        private string _educationalInstitutionRegion = "";
        private string _educationalInstitutionSettlement = "";
        private string _educationalInstitutionName = "";
        private VerticalStackLayout _educationalInstitutionVerticalStackLayout = new()
        {
            Spacing = PageConstants.SPACING_ALL_PAGES
        };
        private ScrollView CreateEducationalInstitutionListView()
        {
            var verticalStack = new VerticalStackLayout
            {
                // Положение
                Padding = PageConstants.PADDING_ALL_PAGES,
                Spacing = PageConstants.SPACING_ALL_PAGES,
            };

            var scrollView = new ScrollView()
            {
                // Доп инфа
                BindingContext = ViewType.EducationalInstitutionList,

                Content = verticalStack
            };

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

            AddLineElems(
                readOnly: true,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Область",
                placeholder: "Минская область",
                textChangedAction: newText => _educationalInstitutionRegion = newText
            );

            AddLineElems(
                readOnly: true,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Населённый пункт",
                placeholder: "Солигорск",
                textChangedAction: newText => _educationalInstitutionSettlement = newText
            );

            AddLineElems(
                readOnly: true,
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

            verticalStack.Add(_educationalInstitutionVerticalStackLayout);

            return scrollView;
        }

        // Получение списка школ
        private List<EducationalInstitutionDTO> _educationalInstitutionDTOList = new();
        private async Task GetEducationalInstitutionList()
        {
            var response = await EducationalInstitutionControl.GetSchools();
            if (response.Error)
            {
                await DisplayAlert("Ошибка", response.Message, "OK");
            }
            else
            {
                _educationalInstitutionDTOList = JsonSerializer.Deserialize<List<EducationalInstitutionDTO>>(response.Message, _jsonSerializerOptions) ?? new List<EducationalInstitutionDTO>();
                _educationalInstitutionDTOList = _educationalInstitutionDTOList
                    .Where(e =>
                        (_educationalInstitutionRegion?.Length == 0 || e.Settlement.Region.Name.Contains(_educationalInstitutionRegion ?? "", StringComparison.OrdinalIgnoreCase)) &&
                        (_educationalInstitutionSettlement?.Length == 0 || e.Settlement.Name.Contains(_educationalInstitutionSettlement ?? "", StringComparison.OrdinalIgnoreCase)) &&
                        (_educationalInstitutionName?.Length == 0 || e.Name.Contains(_educationalInstitutionName ?? "", StringComparison.OrdinalIgnoreCase)))
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

                        // Доп инфа
                        BindingContext = _educationalInstitutionDTOList[i].Id,
                    };
                    grid.GestureRecognizers.Add(tapGesture);

                    var rowIndex = 0;

                    AddLineElems(
                        readOnly: true,
                        grid: grid,
                        startColumn: 0,
                        startRow: rowIndex++,
                        title: "Название",
                        value: _educationalInstitutionDTOList[i].Name
                    );

                    AddLineElems(
                        readOnly: true,
                        grid: grid,
                        startColumn: 0,
                        startRow: rowIndex++,
                        title: "Регион",
                        value: _educationalInstitutionDTOList[i].Settlement.Region.Name
                    );

                    AddLineElems(
                        readOnly: true,
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

        // Действия с отдельными школами
        private async void GetEducationalInstitutionListGestureTapped(object? sender, EventArgs e)
        {
            string action = await DisplayActionSheet(
                "Выберите действие",    // Заголовок
                "Отмена",               // Кнопка отмены
                null,                   // Кнопка деструктивного действия (например, удаление)
                "Описание",             // Остальные кнопки
                "Перейти",
                "Редактировать",
                "Удалить");

            if (sender is not Grid grid)
            {
                return;
            }
            var id = (int)(grid).BindingContext;


            ScrollView scrollView;
            switch (action)
            {
                case "Описание":
                    scrollView = CreateEducationalInstitutionView(id);
                    if ((ViewType)_viewList[^1].BindingContext == ViewType.EducationalInstitution)
                    {
                        _viewList.RemoveAt(_viewList.Count - 1);
                    }
                    _viewList.Add(scrollView);
                    break;
                case "Перейти":

                    break;
                case "Редактировать":
                    scrollView = CreateEducationalInstitutionView(id);
                    if ((ViewType)_viewList[^1].BindingContext == ViewType.EducationalInstitution)
                    {
                        _viewList.RemoveAt(_viewList.Count - 1);
                    }
                    _viewList.Add(scrollView);
                    break;
                case "Удалить":
                    var response = EducationalInstitutionControl.DeleteEducationalInstitution(id).Result;
                    if (response.Error)
                    {
                        await DisplayAlert("Ошибка", response.Message, "OK");
                    }
                    break;
                default:
                    return;
            }

            await GetEducationalInstitutionList();
            RepaintPage();
        }
        private void AddEducationalInstitutionButtonClicked(object? sender, EventArgs e)
        {
            var scrollView = CreateEducationalInstitutionView();
            if ((ViewType)_viewList[^1].BindingContext == ViewType.EducationalInstitution)
            {
                _viewList.RemoveAt(_viewList.Count - 1);
            }
            _viewList.Add(scrollView);
        }
        private EducationalInstitutionDTO? _educationalInstitution = null;
        private ScrollView CreateEducationalInstitutionView(int id = -1, bool edit = false)
        {
            var verticalStack = new VerticalStackLayout
            {
                // Положение
                Padding = PageConstants.PADDING_ALL_PAGES,
                Spacing = PageConstants.SPACING_ALL_PAGES,
            };

            var scrollView = new ScrollView()
            {
                // Доп инфа
                BindingContext = ViewType.EducationalInstitution,

                Content = verticalStack
            };

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

            bool readOnly;
            if (id == -1)
            {
                _educationalInstitution = new();
                readOnly = false;
            }
            else
            {
                _educationalInstitution = _educationalInstitutionDTOList.FirstOrDefault(x => x.Id == id);
                if (_educationalInstitution == null)
                {
                    return scrollView;
                }
                readOnly = true;
            }

            var rowIndex = 0;

            AddLineElems(
                readOnly: readOnly,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Название",
                value: _educationalInstitution.Name,
                placeholder: "",
                textChangedAction: newText => _educationalInstitution.Name = newText
            );

            AddLineElems(
                readOnly: readOnly,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Регион",
                value: _educationalInstitution.Settlement.Region.Name
            );

            AddLineElems(
                readOnly: readOnly,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Населённый пункт",
                value: _educationalInstitution.Settlement.Name
            );

            AddLineElems(
                readOnly: readOnly,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Адресс",
                value: _educationalInstitution.Address
            );

            AddLineElems(
                readOnly: readOnly,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Email",
                value: _educationalInstitution.Email
            );

            AddLineElems(
                readOnly: readOnly,
                grid: grid,
                startColumn: 0,
                startRow: rowIndex++,
                title: "Телефон",
                value: _educationalInstitution.PhoneNumber
            );

            verticalStack.Add(grid);
            return scrollView;
        }
    }
}
