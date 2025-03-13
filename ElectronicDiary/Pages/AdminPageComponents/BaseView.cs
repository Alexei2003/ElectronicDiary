using ElectronicDiary.Pages.Otherts;
using ElectronicDiary.SaveData;
using ElectronicDiary.Web.Api;
using System.Text.Json;

namespace ElectronicDiary.Pages.AdminPageComponents
{
    public interface IMainViewCreator
    {
        ScrollView CreateMainView();
    }

    public class BaseView<TResponse, TRequest, TController> : IMainViewCreator where TController : IController
    {
        protected readonly HorizontalStackLayout _mainStack;
        protected readonly List<ScrollView> _viewList;
        protected VerticalStackLayout _listVerticalStack;
        protected TController _controller;
        protected int _maxCountViews;
        protected long _educationalInstitutionId;

        public BaseView(HorizontalStackLayout mainStack, List<ScrollView> viewList)
        {
            _mainStack = mainStack;
            _viewList = viewList;
            _educationalInstitutionId = -1;
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
            while (_viewList.Count > _maxCountViews)
            {
                _viewList.RemoveAt(_viewList.Count - 1);
            }

            _viewList.Add(scrollView);
            AdminPageStatic.RepaintPage(_mainStack, _viewList);
        }



        // Получение списка объектов
        protected List<TResponse> _objectsList = new();
        protected virtual async Task CreateListView()
        {
            var response = await _controller.GetAll(_educationalInstitutionId);
            if (response != null)
            {
                _objectsList = JsonSerializer.Deserialize<List<TResponse>>(response, PageConstants.JsonSerializerOptions) ?? new List<TResponse>();
            }
        }



        // Действия с отдельными объектами
        protected virtual async void GestureTapped(object? sender, EventArgs e)
        {
            string action;

            if (_maxCountViews == 2)
            {
                action = await Application.Current.Windows[0].Page.DisplayActionSheet(
                    "Выберите действие",    // Заголовок
                    "Отмена",               // Кнопка отмены
                    null,                   // Кнопка деструктивного действия (например, удаление)
                    "Описание",             // Остальные кнопки
                    "Перейти",
                    "Редактировать",
                    "Удалить");
            }
            else
            {
                action = await Application.Current.Windows[0].Page.DisplayActionSheet(
                    "Выберите действие",    // Заголовок
                    "Отмена",               // Кнопка отмены
                    null,                   // Кнопка деструктивного действия (например, удаление)
                    "Описание",             // Остальные кнопки
                    "Редактировать",
                    "Удалить");
            }

            if (sender is not Grid grid)
            {
                return;
            }
            var id = (long)grid.BindingContext;

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
            while (_viewList.Count >= _maxCountViews)
            {
                _viewList.RemoveAt(_viewList.Count - 1);
            }
            _viewList.Add(scrollView);
        }

        protected virtual async Task MoveTo(long id)
        {
            var action = await Application.Current.Windows[0].Page.DisplayActionSheet(
                "Выберите список",      // Заголовок
                "Отмена",               // Кнопка отмены
                null,                   // Кнопка деструктивного действия (например, удаление)
                "Администраторы",       // Остальные кнопки
                "Учителя",
                "Классы",
                "Ученики",
                "Родители");

            IMainViewCreator view;
            switch (action)
            {
                case "Администраторы":
                    view = new AdministratorView(_mainStack, _viewList, id);
                    break;
                case "Учителя":
                    view = new AdministratorView(_mainStack, _viewList, id);
                    break;
                case "Классы":
                    view = new AdministratorView(_mainStack, _viewList, id);
                    break;
                case "Ученики":
                    view = new AdministratorView(_mainStack, _viewList, id);
                    break;
                case "Родители":
                    view = new AdministratorView(_mainStack, _viewList, id);
                    break;
                default:
                    return;
            }
           
            while (_viewList.Count >= _maxCountViews)
            {
                _viewList.RemoveAt(_viewList.Count - 1);
            }
            _viewList.Add(view.CreateMainView());
        }

        protected virtual async Task Edit(long id)
        {
            var scrollView = CreateObjectView(id, true);
            while (_viewList.Count >= _maxCountViews)
            {
                _viewList.RemoveAt(_viewList.Count - 1);
            }
            _viewList.Add(scrollView);
        }

        protected virtual async Task Delete(long id)
        {
            await _controller.Delete(id);
        }



        // Вид объекта
        protected TResponse? _response;
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
            if (id == -1 || edit)
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

        protected TRequest? _request;
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
