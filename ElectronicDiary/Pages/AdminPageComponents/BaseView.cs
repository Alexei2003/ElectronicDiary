using ElectronicDiary.Pages.Otherts;
using ElectronicDiary.SaveData;
using ElectronicDiary.Web.Api;
using System.Text.Json;

namespace ElectronicDiary.Pages.AdminPageComponents
{
    public class BaseView<Object, Request>
    {
        protected readonly HorizontalStackLayout _mainStack;
        protected readonly List<ScrollView> _viewList;
        protected VerticalStackLayout _listVerticalStack;
        protected Controller _controller;
        protected AdminPageStatic.ViewType _objectViewType;

        public BaseView(HorizontalStackLayout mainStack, List<ScrollView> viewList)
        {
            _mainStack = mainStack;
            _viewList = viewList;
        }

        public ScrollView CreateMainView()
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
                BindingContext = AdminPageStatic.ViewType.EducationalInstitutionList,

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

            CreateFilterView(verticalStack, grid);

            var addButton = new Button
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
            addButton.Clicked += AddButtonClicked;
            verticalStack.Add(addButton);

            _listVerticalStack = new()
            {
                Spacing = PageConstants.SPACING_ALL_PAGES
            };
            verticalStack.Add(_listVerticalStack);

            CreateListView();

            return scrollView;
        }

        protected virtual void CreateFilterView(VerticalStackLayout verticalStack, Grid grid, int rowIndex = 0)
        {
            var getButton = new Button
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
            getButton.Clicked += GetButtonClicked;
            grid.Add(getButton, 0, rowIndex);
            verticalStack.Add(grid);
        }

        protected virtual async void GetButtonClicked(object? sender, EventArgs e)
        {
            await CreateListView();
        }

        protected virtual void AddButtonClicked(object? sender, EventArgs e)
        {
            var scrollView = CreateObjectView();
            if ((AdminPageStatic.ViewType)_viewList[^1].BindingContext == _objectViewType)
            {
                _viewList.RemoveAt(_viewList.Count - 1);
            }
            _viewList.Add(scrollView);
            AdminPageStatic.RepaintPage(_mainStack, _viewList);
        }



        // Получение списка объектов
        protected List<Object> _objectsList = new();
        protected virtual async Task CreateListView()
        {
            var response = await _controller.GetAll();
            if (response != null)
            {
                _objectsList = JsonSerializer.Deserialize<List<Object>>(response, PageConstants.JsonSerializerOptions) ?? new List<Object>();
            }
        }



        // Действия с отдельными объектами
        protected virtual async void GestureTapped(object? sender, EventArgs e)
        {
            string action = await Application.Current.Windows[0].Page.DisplayActionSheet(
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
            var id = (int)grid.BindingContext;

            ScrollView scrollView;
            switch (action)
            {
                case "Описание":
                    await ShowInfo(id);
                    break;
                case "Перейти":
                    await MoveTo(id);
                    break;
                case "Редактировать":
                    await Edit(id);
                    break;
                case "Удалить":
                    await Delete(id);
                    break;
                default:
                    return;
            }

            await CreateListView();
            AdminPageStatic.RepaintPage(_mainStack, _viewList);
        }

        protected virtual async Task ShowInfo(long id)
        {
            var scrollView = CreateObjectView(id);
            if ((AdminPageStatic.ViewType)_viewList[^1].BindingContext == _objectViewType)
            {
                _viewList.RemoveAt(_viewList.Count - 1);
            }
            _viewList.Add(scrollView);
        }

        protected virtual async Task MoveTo(long id)
        {

        }

        protected virtual async Task Edit(long id)
        {

        }

        protected virtual async Task Delete(long id)
        {
            await _controller.Delete(id);
        }



        // Вид объекта
        protected virtual ScrollView CreateObjectView(long id = -1, bool edit = false)
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
                BindingContext = AdminPageStatic.ViewType.EducationalInstitution,

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

            CreateObjectInfoView(verticalStack, grid, 0, id, edit);

            verticalStack.Add(grid);
            return scrollView;
        }

        protected virtual void CreateObjectInfoView(VerticalStackLayout verticalStack, Grid grid, int rowIndex = 0, long id = -1, bool edit = false)
        {
            if (id == -1)
            {
                var saveButton = new Button
                {
                    // Положение
                    HorizontalOptions = LayoutOptions.Center,

                    // Цвета
                    BackgroundColor = UserData.UserSettings.Colors.ACCENT_COLOR,
                    TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                    // Текст
                    FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                    Text = "Сохранить",
                };
                saveButton.Clicked += SaveButtonClicked;

                grid.Add(saveButton, 0, rowIndex);
            }
        }

        protected Request _request;
        protected virtual async void SaveButtonClicked(object? sender, EventArgs e)
        {
            if (_request == null)
            {
                return;
            }

            var json = JsonSerializer.Serialize(_request, PageConstants.JsonSerializerOptions);

            var response = await _controller.Add(json);
            if (response != null)
            {
                await Application.Current.Windows[0].Page.DisplayAlert("Успех", "Объект сохранён", "OK");
                await CreateListView();
                AdminPageStatic.OnBackButtonPressed(_mainStack, _viewList);
            }
        }
    }
}
