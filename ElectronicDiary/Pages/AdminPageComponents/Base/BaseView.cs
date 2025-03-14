using ElectronicDiary.Pages.Otherts;
using ElectronicDiary.SaveData;
using ElectronicDiary.Web.Api;
using ElectronicDiary.Web.DTO.Responses;
using System.Text.Json;

namespace ElectronicDiary.Pages.AdminPageComponents.Base
{
    public interface IBaseView
    {
        ScrollView CreateMainView();
    }

    public class BaseView<TResponse, TRequest, TController> : IBaseView
        where TController : IController
        where TResponse : BaseResponse
    {
        protected readonly HorizontalStackLayout _mainStack;
        protected readonly List<ScrollView> _viewList;
        protected VerticalStackLayout _listVerticalStack;
        protected TController _controller;
        protected int _maxCountViews;
        protected long _educationalInstitutionId;
        protected long _elemId;

        public BaseView(HorizontalStackLayout mainStack, List<ScrollView> viewList)
        {
            _mainStack = mainStack;
            _viewList = viewList;
            _educationalInstitutionId = -1;
            _maxCountViews = 0;
        }

        private void DeleteView()
        {
            while (_viewList.Count >= _maxCountViews)
            {
                _viewList.RemoveAt(_viewList.Count - 1);
            }
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

            Grid grid = new Grid
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

            CreateFilterView(grid);
            verticalStack.Add(grid);

            var getButton = new Button
            {
                // Положение
                HorizontalOptions = LayoutOptions.Fill,

                // Цвета
                BackgroundColor = UserData.UserSettings.Colors.ACCENT_COLOR,
                TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                // Текст
                FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                Text = "Найти",
            };
            getButton.Clicked += GetButtonClicked;
            verticalStack.Add(getButton);


            var addButton = new Button
            {
                // Положение
                HorizontalOptions = LayoutOptions.Fill,

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

        // Пусто
        protected virtual void CreateFilterView(Grid grid, int rowIndex = 0)
        {
        }

        protected virtual async void GetButtonClicked(object? sender, EventArgs e)
        {
            await CreateListView();
        }

        protected virtual void AddButtonClicked(object? sender, EventArgs e)
        {
            _elemId = -1;
            var scrollView = CreateObjectView();
            DeleteView();
            _viewList.Add(scrollView);
            AdminPageStatic.RepaintPage(_mainStack, _viewList);
        }



        // Получение списка объектов
        protected List<TResponse> _objectsList = [];
        protected virtual async Task GetList()
        {
            var response = await _controller.GetAll(_educationalInstitutionId);
            if (response != null)
            {
                _objectsList = JsonSerializer.Deserialize<List<TResponse>>(response, PageConstants.JsonSerializerOptions) ?? new List<TResponse>();
            }
            FilterList();
        }

        // Пусто
        protected virtual void FilterList()
        {

        }

        // Пусто
        protected virtual void CreateListElemView(Grid grid, int indexElem, int rowIndex = 0)
        {

        }

        protected virtual async Task CreateListView()
        {
            await GetList();

            _listVerticalStack.Clear();

            for (var i = 0; i < _objectsList.Count; i++)
            {
                var tapGesture = new TapGestureRecognizer();
                tapGesture.Tapped += GestureTapped;
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
                    BindingContext = _objectsList[i].Id,
                };
                grid.GestureRecognizers.Add(tapGesture);

                CreateListElemView(grid, i);

                _listVerticalStack.Add(grid);
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
                    ShowInfo(id);
                    break;
                case "Перейти":
                    await MoveTo(id);
                    break;
                case "Редактировать":
                    Edit(id);
                    break;
                case "Удалить":
                    Delete(id);
                    break;
                default:
                    return;
            }

            await CreateListView();
            AdminPageStatic.RepaintPage(_mainStack, _viewList);
        }

        protected virtual void ShowInfo(long id)
        {
            _elemId = id;
            var scrollView = CreateObjectView();
            DeleteView();
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

            IBaseView view;
            switch (action)
            {
                case "Администраторы":
                    view = new AdministratorView(_mainStack, _viewList, id);
                    break;
                case "Учителя":
                    view = new TeacherView(_mainStack, _viewList, id);
                    break;
                case "Классы":
                    view = new AdministratorView(_mainStack, _viewList, id);
                    break;
                case "Ученики":
                    view = new SchoolStudentView(_mainStack, _viewList, id);
                    break;
                case "Родители":
                    view = new ParentView(_mainStack, _viewList, id);
                    break;
                default:
                    return;
            }

            DeleteView();
            _viewList.Add(view.CreateMainView());
        }

        protected virtual void Edit(long id)
        {
            _elemId = id;
            var scrollView = CreateObjectView(true);
            while (_viewList.Count >= _maxCountViews)
            {
                _viewList.RemoveAt(_viewList.Count - 1);
            }
            _viewList.Add(scrollView);
        }

        protected virtual void Delete(long id)
        {
            _controller.Delete(id);
        }



        // Вид объекта
        protected TResponse? _response;
        protected virtual ScrollView CreateObjectView(bool edit = false)
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

            CreateObjectInfoView(grid, 0, edit);
            verticalStack.Add(grid);

            if (_elemId == -1 || edit)
            {
                var saveButton = new Button
                {
                    // Положение
                    HorizontalOptions = LayoutOptions.Fill,

                    // Цвета
                    BackgroundColor = UserData.UserSettings.Colors.ACCENT_COLOR,
                    TextColor = UserData.UserSettings.Colors.TEXT_COLOR,

                    // Текст
                    FontSize = UserData.UserSettings.Fonts.BASE_FONT_SIZE,
                    Text = "Сохранить",
                };
                saveButton.Clicked += SaveButtonClicked;

                verticalStack.Add(saveButton);
            }

            return scrollView;
        }

        protected AdminPageStatic.ComponentType _componentTypeEntity;
        protected AdminPageStatic.ComponentType _componentTypePicker;
        protected virtual void CreateObjectInfoView(Grid grid, int rowIndex = 0, bool edit = false)
        {
            if (_elemId != -1)
            {
                _response = _objectsList.FirstOrDefault(x => x.Id == _elemId);
            }

            if (edit || _elemId == -1)
            {
                _componentTypeEntity = AdminPageStatic.ComponentType.Entity;
                _componentTypePicker = AdminPageStatic.ComponentType.Picker;
            }
            else
            {
                _componentTypeEntity = AdminPageStatic.ComponentType.Label;
                _componentTypePicker = AdminPageStatic.ComponentType.Label;
            }
        }

        protected TRequest? _request;
        protected virtual async void SaveButtonClicked(object? sender, EventArgs e)
        {
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
